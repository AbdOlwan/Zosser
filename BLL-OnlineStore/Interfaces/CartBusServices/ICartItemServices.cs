using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.CartBusServices
{
    public interface ICartItemServices
    {
        Task<List<CartItemDTO>?> GetAllCartItems();
        Task<CartItemDTO?> AddNewCartItem(CartItemDTO CartItem);

        Task<int> CountCartItems();

        Task<CartItemDTO?> GetCartItemById(int id);

        Task<bool> DeleteCartItemById(int id);

        Task<bool> UpdateCartItemById(CartItemDTO CartItem);

    }
}
