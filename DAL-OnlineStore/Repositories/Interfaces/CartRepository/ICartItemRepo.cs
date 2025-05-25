using DAL_OnlineStore.Entities.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.CartItemRepository
{
    public interface ICartItemRepo
    {
        Task<List<CartItem>?> getAllCartItems();
        Task<CartItem> addNewCartItem(CartItem CartItem);

        Task<int> countCartItems();

        Task<CartItem?> getCartItemById(int id);

        Task<bool> deleteCartItemById(int id);

        Task<bool> updateCartItemById(CartItem CartItem);

    }
}
