using DAL.Entities.Models.OrderModels;
using Shared.Constants;
using Shared.DTOs.Order;

namespace DAL.Repositories.Interfaces.Order_Interfaces
{
    public interface IOrderRepo
    {

        Task<Order?> GetByIdAsync(int id);
        Task<PagedResponse<Order>> GetAllAsync(int pageNumber, int pageSize, OrderStatus? status);

        Task<int> GetPendingOrdersCountAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);

        // Specific Business Logic
        Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
        Task<bool> ExistsAsync(int id);

       
        Task<bool> SaveChangesAsync();
    }
}