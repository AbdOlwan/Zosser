using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces.OrderRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.OrderRepository
{
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly AppDbContext _context;

        public OrderItemRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(i => i.OrderID == orderId)
                .Include(i => i.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
        {
            return await _context.OrderItems
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.OrderItemID == id);
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem item)
        {
            await _context.OrderItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateOrderItemAsync(OrderItem item)
        {
            _context.OrderItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item != null)
            {
                _context.OrderItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> OrderItemExistsAsync(int id)
        {
            return await _context.OrderItems.AnyAsync(i => i.OrderItemID == id);
        }
    }

}
