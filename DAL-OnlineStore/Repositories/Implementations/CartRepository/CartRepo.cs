using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Repositories.Interfaces.CartRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.CartRepository
{
    public class CartRepo : ICartRepo
    {
        private readonly AppDbContext _context;

        public CartRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cart>?> getAllCarts()
        {
            return await _context.Carts
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<Cart?> getCartById(int Id)
        {
            return await _context.Carts.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.CartId == Id);
        }

        public async Task<Cart> addNewCart(Cart Cart)
        {
            await _context.Carts.AddAsync(Cart);
            await _context.SaveChangesAsync();
            return Cart;
        }
        public async Task<bool> deleteCartById(int id)
        {
            var Cart = await _context.Carts.FirstOrDefaultAsync(d => d.CartId == id);
            if (Cart == null)
            {
                return false;
            }

            _context.Carts.Remove(Cart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateCartById(Cart Cart)
        {
            var result = await _context.Carts.FirstOrDefaultAsync(d => d.CartId == Cart.CartId);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(Cart);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //public async Task<int> countCarts()
        //{
        //    return await _context.Carts.CountAsync();
        //}
    
    }
}
