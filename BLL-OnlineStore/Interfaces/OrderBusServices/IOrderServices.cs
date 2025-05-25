using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.OrderBusServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO?> GetOrderByIdAsync(int id);
        Task<OrderDTO> CreateOrderAsync(OrderDTO createOrderDTO);
        Task UpdateOrderAsync(OrderDTO updateOrderDTO);
        Task DeleteOrderAsync(int id);
        Task<bool> OrderExistsAsync(int id);
    }

}
