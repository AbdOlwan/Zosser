using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using BLL_OnlineStore.Interfaces.OrderBusServices;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Repositories.Interfaces;

using DAL_OnlineStore.Entities.Models.ShipmentModels;

namespace DAL_OnlineStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO?> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdAsync(id);
            if (order == null) return null;
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderDTO dto)
        {
            // خريطة CreateOrderDTO إلى كيان Order مرتبطًا بالكيانات الفرعية
            var order = new Order
            {
                CustomerID = dto.CustomerID,
                OrderDate = dto.OrderDate,
                OrderStatus = dto.OrderStatus,

                // Snapshot address
                OrderAddress = _mapper.Map<OrderAddress>(dto.OrderAddress),

                // Payment & Shipment
                Payment = _mapper.Map<Payment>(dto.Payment),
                Shipment = _mapper.Map<Shipment>(dto.Shipment),

                // Items
                Items = _mapper.Map<ICollection<OrderItem>>(dto.Items),

                // Financials
                SubtotalAmount = dto.SubtotalAmount,
                TaxAmount = dto.TaxAmount,
                ShippingAmount = dto.ShippingAmount,
                DiscountAmount = dto.DiscountAmount,
                TotalAmount = dto.TotalAmount,


                // Notes
                UserNotes = dto.UserNotes,
                InternalNotes = dto.InternalNotes,

                // Audit
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.CreatedBy // تأكد إضافة CreatedBy إلى DTO
            };

            // أضف الجذر فقط، EF Core سيقوم بإدراج الرسم البياني كاملاً
            await _unitOfWork.Orders.AddOrderAsync(order);

            // احفظ التغييرات كوحدة عمل واحدة
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task UpdateOrderAsync(OrderDTO dto)
        {
            // خريطة UpdateOrderDTO إلى كيان Order
            var order = _mapper.Map<Order>(dto);

            // حدِّث الكيان في الـ DbContext
            await _unitOfWork.Orders.UpdateOrderAsync(order);

            // احفظ كوحدة عمل
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            // علامة الحذف المنطقي أو الحذف الفعلي
            await _unitOfWork.Orders.DeleteOrderAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _unitOfWork.Orders.OrderExistsAsync(id);
        }
    }
}

