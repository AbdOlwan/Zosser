using BLL.Exceptions;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.Order;

[Route("api/v1/orders")]
[ApiController]
// [Authorize] // جميع عمليات الطلبات تتطلب تسجيل الدخول
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }




    /// <summary>
    /// الحصول على جميع الطلبات مع دعم تقسيم الصفحات والفلترة.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = Policies.RequireAdminRole)]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<OrderDto>>), 200)] 
    public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = DefaultValues.DefaultPageSize,
        [FromQuery] OrderStatus? status = null)
    {
        var pagedOrders = await _orderService.GetAllOrdersAsync(pageNumber, pageSize, status);

        if (pagedOrders == null || !pagedOrders.Items.Any())
        {
            return ApiResponse<PagedResponse<OrderDto>>
                .CreateSuccess(pagedOrders!, "لا توجد طلبات متاحة تطابق البحث.") 
                .ToActionResult();
        }

        return ApiResponse<PagedResponse<OrderDto>>
            .CreateSuccess(pagedOrders, ApiMessages.Success) 
            .ToActionResult();
    }





    /// <summary>
    /// الحصول على عدد الطلبات الجديدة (المعلقة).
    /// </summary>
    [HttpGet("status/pending/count")]
    [Authorize(Policy = Policies.RequireAdminRole)]
    [ProducesResponseType(typeof(ApiResponse<int>), 200)]
    public async Task<IActionResult> GetPendingOrdersCount()
    {
        try
        {
            var count = await _orderService.GetPendingOrdersCountAsync();
            return ApiResponse<int>
                .CreateSuccess(count, ApiMessages.Success) // استخدام رسالة النجاح من الثوابت
                .ToActionResult();
        }
        catch (Exception )
        {
            return ApiResponse<int>
                .CreateInternalServerError(ApiMessages.InternalServerError) // استخدام رسالة الخطأ من الثوابت
                .ToActionResult();
        }
    }




    /// <summary>
    /// الحصول على طلب واحد بواسطة المعرف (ID).
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetOrderById(int id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return ApiResponse<OrderDto>
                    .CreateNotFound($"الطلب رقم {id} غير موجود")
                    .ToActionResult();
            }

            return ApiResponse<OrderDto>
                .CreateSuccess(order, "تم جلب تفاصيل الطلب بنجاح")
                .ToActionResult();
        }
        catch (NotFoundException ex)
        {
            return ApiResponse<OrderDto>
                .CreateNotFound(ex.Message)
                .ToActionResult();
        }
        catch (Exception )
        {
            return ApiResponse<OrderDto>
                .CreateInternalServerError("حدث خطأ أثناء جلب تفاصيل الطلب")
                .ToActionResult();
        }
    }




    /// <summary>
    /// الحصول على تفاصيل طلب معين بواسطة المعرف (ID).
    /// </summary>
    [HttpGet("{id}/details")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponse<OrderDetailsDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetOrderDetails(int id)
    {
        try
        {
            var orderDetails = await _orderService.GetOrderWithDetailsAsync(id);
            if (orderDetails == null)
            {
                return ApiResponse<OrderDetailsDto>
                    .CreateNotFound($"تفاصيل الطلب رقم {id} غير موجودة")
                    .ToActionResult();
            }
            return ApiResponse<OrderDetailsDto>
                .CreateSuccess(orderDetails, "تم جلب تفاصيل الطلب بنجاح")
                .ToActionResult();
        }
        catch (NotFoundException ex)
        {
            return ApiResponse<OrderDetailsDto>
                .CreateNotFound(ex.Message)
                .ToActionResult();
        }
        catch (Exception )
        {
            return ApiResponse<OrderDetailsDto>
                .CreateInternalServerError("حدث خطأ أثناء جلب تفاصيل الطلب")
                .ToActionResult();
        }
    }




    /// <summary>
    /// إنشاء طلب جديد.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = Policies.RequireCustomerRole)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderForCreationDto orderDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse<OrderDto>
                    .CreateBadRequest(ModelState)
                    .ToActionResult();
            }

            var createdOrder = await _orderService.CreateOrderAsync(orderDto);
            if (createdOrder == null)
            {
                return ApiResponse<OrderDto>
                    .CreateBadRequest("فشل في إنشاء الطلب - بيانات غير صالحة")
                    .ToActionResult();
            }

            return ApiResponse<OrderDto>
                .CreateCreated(createdOrder, "تم إنشاء الطلب بنجاح")
                .WithRequestId(HttpContext.TraceIdentifier)
                .ToActionResult();
        }
        catch (BusinessException ex) // **إضافة جديدة: لمعالجة أخطاء منطق العمل**
        {
            return ApiResponse<OrderDto>
                .CreateBadRequest(ex.Message) // إرجاع خطأ 400 مع رسالة واضحة
                .ToActionResult();
        }
        catch (NotFoundException ex)
        {
            return ApiResponse<OrderDto>
                .CreateNotFound(ex.Message)
                .ToActionResult();
        }
        catch (Exception )
        {
            return ApiResponse<OrderDto>
                .CreateInternalServerError("حدث خطأ أثناء إنشاء الطلب")
                .ToActionResult();
        }
    }




    /// <summary>
    /// تحديث طلب موجود بواسطة المعرف (ID).
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderForUpdateDto orderDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse
                    .CreateBadRequest(ModelState)
                    .ToActionResult();
            }

            await _orderService.UpdateOrderAsync(id, orderDto);
            return ApiResponse
                .CreateNoContent("تم تحديث الطلب بنجاح")
                .ToActionResult();
        }
        catch (NotFoundException ex)
        {
            return ApiResponse
                .CreateNotFound(ex.Message)
                .ToActionResult();
        }
        catch (Exception )
        {
            return ApiResponse
                .CreateInternalServerError("حدث خطأ أثناء تحديث الطلب")
                .ToActionResult();
        }
    }




    /// <summary>
    /// حذف طلب بواسطة المعرف (ID).
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            await _orderService.DeleteOrderAsync(id);
            return ApiResponse
                .CreateNoContent("تم حذف الطلب بنجاح")
                .ToActionResult();
        }
        catch (NotFoundException ex)
        {
            return ApiResponse
                .CreateNotFound(ex.Message)
                .ToActionResult();
        }
        catch (Exception )
        {
            return ApiResponse
                .CreateInternalServerError("حدث خطأ أثناء حذف الطلب")
                .ToActionResult();
        }
    }




    /// <summary>
    /// الحصول على طلبات عميل معين بواسطة معرف العميل (CustomerId).
    /// </summary>
    [HttpGet("customer/{customerId}")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetCustomerOrders(int customerId)
    {
        try
        {
            var orders = await _orderService.GetCustomerOrdersAsync(customerId);

            if (orders == null || !orders.Any())
            {
                return ApiResponse<IEnumerable<OrderDto>>
                    .CreateSuccess(new List<OrderDto>(), $"لا توجد طلبات للعميل رقم {customerId}")
                    .ToActionResult();
            }

            return ApiResponse<IEnumerable<OrderDto>>
                .CreateSuccess(orders, "تم جلب طلبات العميل بنجاح")
                .WithRequestId(HttpContext.TraceIdentifier)
                .ToActionResult();
        }
        catch (NotFoundException ex)
        {
            return ApiResponse<IEnumerable<OrderDto>>
                .CreateNotFound(ex.Message)
                .ToActionResult();
        }
        catch (Exception )
        {
            return ApiResponse<IEnumerable<OrderDto>>
                .CreateInternalServerError("حدث خطأ أثناء جلب طلبات العميل")
                .ToActionResult();
        }
    }




    [HttpPut("{orderId}/status")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public  async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatus newStatus)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse
                    .CreateBadRequest(ModelState)
                    .ToActionResult();
            }
            await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            return ApiResponse
                .CreateNoContent("تم تحديث حالة الطلب بنجاح")
                .ToActionResult();
        }
        catch (NotFoundException ex)
        {
            return ApiResponse
                .CreateNotFound(ex.Message)
                .ToActionResult();
        }
        catch (Exception )
        {
            return ApiResponse
                .CreateInternalServerError("حدث خطأ أثناء تحديث حالة الطلب")
                .ToActionResult();
        }
    }
}