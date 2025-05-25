using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.CartRepository
{
    public interface ICartRepo
    {
        Task<List<Cart>?> getAllCarts();
        Task<Cart> addNewCart(Cart Cart);

        //Task<int> countCarts();

        Task<Cart?> getCartById(int id);

        Task<bool> deleteCartById(int id);

        Task<bool> updateCartById(Cart Cart);
    }
}
