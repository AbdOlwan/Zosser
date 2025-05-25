using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.CartBusServices
{
    public interface ICartServices
    {
        Task<List<CartDTO>?> GetAllCarts();
        Task<CartDTO?> AddNewCart(CartDTO Cart);

       // Task<int> CountCarts();

        Task<CartDTO?> GetCartById(int id);

        Task<bool> DeleteCartById(int id);

        Task<bool> UpdateCartById(CartDTO Cart);

    }
}
