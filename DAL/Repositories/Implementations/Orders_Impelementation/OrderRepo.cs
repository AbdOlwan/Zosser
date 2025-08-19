using DAL.Context;
using DAL.Entities.Models.OrderModels;
using DAL.Repositories.Interfaces.Order_Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.DTOs.Order;

namespace DAL.Repositories.Implementations.Orders_Impelementation
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<PagedResponse<Order>> GetAllAsync(int pageNumber, int pageSize, OrderStatus? status)
        {
            IQueryable<Order> query = _context.Orders
                                              .Include(o => o.Customer)
                                              .Include(o => o.Product)
                                              .OrderByDescending(o => o.OrderDate);

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // نستخدم الآن PagedResponse بدلاً من PagedResult
            return new PagedResponse<Order>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber, // اسم الحقل هنا PageNumber
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<int> GetPendingOrdersCountAsync()
        {
            // دالة خفيفة وسريعة جداً، تقوم فقط بعدّ الطلبات التي حالتها "قيد المراجعة"
            return await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
        }

        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                 .ThenInclude(c=>c.User)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(e => e.Id == id);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}