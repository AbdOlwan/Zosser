using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.OrderBusServices
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDTO>> GetItemsByOrderIdAsync(int orderId);
        Task<OrderItemDTO?> GetOrderItemByIdAsync(int id);
        Task<OrderItemDTO> CreateOrderItemAsync(OrderItemDTO createOrderItemDTO);
        Task UpdateOrderItemAsync(OrderItemDTO updateOrderItemDTO);
        Task DeleteOrderItemAsync(int id);
        Task<bool> OrderItemExistsAsync(int id);
    }

}
