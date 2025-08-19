using BLL.Exceptions;
using BLL.Services.Interfaces;
using BLL.Services.Interfaces.Payments;
using BLL.Services.Interfaces.Payments.Wallets;
using DAL.Entities.Models.OrderModels;
using DAL.Repositories.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DTOs.Order;
using Shared.DTOs.Payments;

namespace BLL.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWalletService _walletService;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IUnitOfWork unitOfWork, IWalletService walletService, IPaymentService paymentService, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _walletService = walletService;
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<PagedResponse<OrderDto>?> GetAllOrdersAsync(int pageNumber, int pageSize, OrderStatus? status)
        {
            var pagedOrders = await _unitOfWork.Orders.GetAllAsync(pageNumber, pageSize, status);
            if (pagedOrders == null) return null;

            // Mapster ذكي كفاية ليعرف كيف يحول من PagedResponse<Order> إلى PagedResponse<OrderDto>
            var pagedOrderDtos = pagedOrders.Adapt<PagedResponse<OrderDto>>();

            return pagedOrderDtos;
        }
        public async Task<int> GetPendingOrdersCountAsync()
        {
            return await _unitOfWork.Orders.GetPendingOrdersCountAsync();
        }
        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }
            return order.Adapt<OrderDto>();
        }

        public async Task<OrderDetailsDto?> GetOrderWithDetailsAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException($"الطلب رقم {orderId} غير موجود");
            }
            return order.Adapt<OrderDetailsDto>();
        }

        public async Task<OrderDto?> CreateOrderAsync(OrderForCreationDto orderDto)
        {
            await _unitOfWork.BeginTransactionAsync(); // **بدء العملية المتكاملة**
            try
            {
                // الخطوة 1: إنشاء كائن الطلب وتحضيره
                var order = orderDto.Adapt<Order>();
                order.OrderDate = DateTime.UtcNow;
                order.Status = OrderStatus.Pending; // الحالة الأولية

                // الخطوة 2: إضافة الطلب إلى السياق وحفظه للحصول على ID
                // داخل الـ Transaction، هذا الحفظ ليس نهائياً (Commit)
                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync(); // ضروري للحصول على order.Id

                _logger.LogInformation("Order {OrderId} created in transaction, processing payment.", order.Id);

                // الخطوة 3: معالجة الدفع بناءً على الطريقة المختارة
                switch (orderDto.PaymentMethod)
                {
                    case PaymentMethod.ZosserWallet:
                        // استدعاء خدمة المحفظة لخصم المبلغ
                        await _walletService.UseWalletForPurchaseAsync(order.CustomerId, order.TotalAmount, order.Id);
                        await _unitOfWork.SaveChangesAsync();
                        _logger.LogInformation("Successfully processed ZosserWallet payment for Order {OrderId}.", order.Id);
                        break;

                    case PaymentMethod.CashOnDelivery:
                        // إنشاء سجل دفع معلق لتوحيد آلية العمل
                        var paymentDto = new PaymentCreateDTO
                        {
                            OrderId = order.Id,
                            Amount = order.TotalAmount,
                            Method = PaymentMethod.CashOnDelivery,
                        };
                        await _paymentService.CreatePaymentAsync(paymentDto);
                        await _unitOfWork.SaveChangesAsync();
                        _logger.LogInformation("Created pending CashOnDelivery payment record for Order {OrderId}.", order.Id);
                        break;

                    // يمكنك إضافة طرق دفع أخرى هنا مستقبلاً
                    // case PaymentMethod.CreditCard:
                    //     // ...
                    //     break;

                    default:
                        // إذا كانت طريقة الدفع غير مدعومة
                        throw new BusinessException("طريقة الدفع المحددة غير صالحة أو غير مدعومة.");
                }

                // الخطوة 4: إذا نجحت كل الخطوات، قم بتأكيد العملية
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Order {OrderId} and payment successfully committed.", order.Id);

                // إرجاع بيانات الطلب بعد نجاح العملية بالكامل
                return order.Adapt<OrderDto>();
            }
            catch (Exception ex)
            {
                // الخطوة 5: في حالة حدوث أي خطأ، تراجع عن كل شيء
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create order. Transaction rolled back. Reason: {ErrorMessage}", ex.Message);

                // أعد رمي الخطأ ليتم معالجته في الـ Controller وإرجاع رسالة مناسبة للمستخدم
                throw;
            }
        }
        public async Task UpdateOrderAsync(int id, OrderForUpdateDto orderDto)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return;
            }

            orderDto.Adapt(order);

            await _unitOfWork.Orders.UpdateAsync(order);
            // حفظ التغييرات في قاعدة البيانات
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return;
            }

            await _unitOfWork.Orders.DeleteAsync(id);
            // حفظ التغييرات في قاعدة البيانات
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(int customerId)
        {
            var orders = await _unitOfWork.Orders.GetCustomerOrdersAsync(customerId);
            return orders.Adapt<IEnumerable<OrderDto>>();
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null || order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Delivered)
            {
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }
            // تحديث حالة الطلب
            order.Status = newStatus;
            // حفظ التغييرات في قاعدة البيانات
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}