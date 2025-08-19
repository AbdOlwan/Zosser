using Shared.Constants;
using Shared.DTOs.Order;

namespace BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResponse<OrderDto>?> GetAllOrdersAsync(int pageNumber, int pageSize, OrderStatus? status);

        Task<int> GetPendingOrdersCountAsync();
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<OrderDetailsDto?> GetOrderWithDetailsAsync(int orderId);
        Task<OrderDto?> CreateOrderAsync(OrderForCreationDto orderDto);
        Task UpdateOrderAsync(int id, OrderForUpdateDto orderDto);
        Task DeleteOrderAsync(int id);
        Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(int customerId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
}