using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Repositories.Interfaces.CartItemRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.CartItemRepository
{
    public class CartItemRepo : ICartItemRepo
    {
        private readonly AppDbContext _context;

        public CartItemRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>?> getAllCartItems()
        {
            return await _context.CartItems
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<CartItem?> getCartItemById(int Id)
        {
            return await _context.CartItems.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.CartItemId == Id);
        }

        public async Task<CartItem> addNewCartItem(CartItem CartItem)
        {
            await _context.CartItems.AddAsync(CartItem);
            await _context.SaveChangesAsync();
            return CartItem;
        }
        public async Task<bool> deleteCartItemById(int id)
        {
            var CartItem = await _context.CartItems.FirstOrDefaultAsync(d => d.CartItemId == id);
            if (CartItem == null)
            {
                return false;
            }

            _context.CartItems.Remove(CartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateCartItemById(CartItem CartItem)
        {
            var result = await _context.CartItems.FirstOrDefaultAsync(d => d.CartItemId == CartItem.CartItemId);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(CartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> countCartItems()
        {
            return await _context.CartItems.CountAsync();
        }
    }
    }
