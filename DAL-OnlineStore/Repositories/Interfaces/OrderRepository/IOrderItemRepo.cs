using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.OrderRepository
{
    public interface IOrderItemRepo
    {
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
        Task<OrderItem?> GetOrderItemByIdAsync(int id);
        Task<OrderItem> AddOrderItemAsync(OrderItem item);
        Task UpdateOrderItemAsync(OrderItem item);
        Task DeleteOrderItemAsync(int id);
        Task<bool> OrderItemExistsAsync(int id);
    }

}
