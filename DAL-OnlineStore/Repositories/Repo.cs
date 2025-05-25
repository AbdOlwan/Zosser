using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories
{
    //public class OrderRepo : IOrderRepo
    //{
    //    private readonly AppDbContext _context;
    //    private readonly IMapper _mapper;

    //    public OrderRepo(AppDbContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    //    {
    //        return await _context.Orders
    //            .Include(o => o.Items)
    //            .Include(o => o.OrderAddress)
    //            .Include(o => o.Payment)
    //            .Include(o => o.Shipment)
    //            .AsNoTracking()
    //            .ToListAsync();
    //    }

    //    public async Task<Order?> GetOrderByIdAsync(int id)
    //    {
    //        return await _context.Orders
    //            .Include(o => o.Items)
    //            .Include(o => o.OrderAddress)
    //            .Include(o => o.Payment)
    //            .Include(o => o.Shipment)
    //            .FirstOrDefaultAsync(o => o.OrderID == id);
    //    }

    //    public async Task<Order> AddOrderAsync(Order order)
    //    {
    //        order.CreatedAt = DateTime.UtcNow;
    //        await _context.Orders.AddAsync(order);
    //        await _context.SaveChangesAsync();
    //        return order;
    //    }

    //    public async Task UpdateOrderAsync(Order order)
    //    {
    //        order.UpdatedAt = DateTime.UtcNow;
    //        _context.Orders.Update(order);
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task DeleteOrderAsync(int id)
    //    {
    //        var order = await _context.Orders.FindAsync(id);
    //        if (order != null)
    //        {
    //            _context.Orders.Remove(order);
    //            await _context.SaveChangesAsync();
    //        }
    //    }

    //    public async Task<bool> OrderExistsAsync(int id)
    //    {
    //        return await _context.Orders.AnyAsync(o => o.OrderID == id);
    //    }

    //    public async Task<IEnumerable<Order>> FindOrdersAsync(Expression<Func<Order, bool>> predicate)
    //    {
    //        return await _context.Orders
    //            .Where(predicate)
    //            .Include(o => o.Items)
    //            .Include(o => o.OrderAddress)
    //            .Include(o => o.Payment)
    //            .Include(o => o.Shipment)
    //            .AsNoTracking()
    //            .ToListAsync();
    //    }
    //}

    //public class OrderItemRepo : IOrderItemRepo
    //{
    //    private readonly AppDbContext _context;

    //    public OrderItemRepo(AppDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
    //    {
    //        return await _context.OrderItems
    //            .Where(i => i.OrderID == orderId)
    //            .Include(i => i.Product)
    //            .AsNoTracking()
    //            .ToListAsync();
    //    }

    //    public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
    //    {
    //        return await _context.OrderItems
    //            .Include(i => i.Product)
    //            .FirstOrDefaultAsync(i => i.OrderItemID == id);
    //    }

    //    public async Task<OrderItem> AddOrderItemAsync(OrderItem item)
    //    {
    //        await _context.OrderItems.AddAsync(item);
    //        await _context.SaveChangesAsync();
    //        return item;
    //    }

    //    public async Task UpdateOrderItemAsync(OrderItem item)
    //    {
    //        _context.OrderItems.Update(item);
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task DeleteOrderItemAsync(int id)
    //    {
    //        var item = await _context.OrderItems.FindAsync(id);
    //        if (item != null)
    //        {
    //            _context.OrderItems.Remove(item);
    //            await _context.SaveChangesAsync();
    //        }
    //    }

    //    public async Task<bool> OrderItemExistsAsync(int id)
    //    {
    //        return await _context.OrderItems.AnyAsync(i => i.OrderItemID == id);
    //    }
    //}

    //public class PaymentRepo : IPaymentRepo
    //{
    //    private readonly AppDbContext _context;

    //    public PaymentRepo(AppDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
    //    {
    //        return await _context.Payments
    //            .Include(p => p.Order)
    //            .AsNoTracking()
    //            .ToListAsync();
    //    }

    //    public async Task<Payment?> GetPaymentByIdAsync(int id)
    //    {
    //        return await _context.Payments
    //            .Include(p => p.Order)
    //            .FirstOrDefaultAsync(p => p.PaymentID == id);
    //    }

    //    public async Task<Payment> AddPaymentAsync(Payment payment)
    //    {
    //        payment.PaymentDate = DateTime.UtcNow;
    //        await _context.Payments.AddAsync(payment);
    //        await _context.SaveChangesAsync();
    //        return payment;
    //    }

    //    public async Task UpdatePaymentAsync(Payment payment)
    //    {
    //        _context.Payments.Update(payment);
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task DeletePaymentAsync(int id)
    //    {
    //        var payment = await _context.Payments.FindAsync(id);
    //        if (payment != null)
    //        {
    //            _context.Payments.Remove(payment);
    //            await _context.SaveChangesAsync();
    //        }
    //    }

    //    public async Task<bool> PaymentExistsAsync(int id)
    //    {
    //        return await _context.Payments.AnyAsync(p => p.PaymentID == id);
    //    }
    //}

    //public class ShipmentRepo : IShipmentRepo
    //{
    //    private readonly AppDbContext _context;

    //    public ShipmentRepo(AppDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<IEnumerable<Shipment>> GetAllShipmentsAsync()
    //    {
    //        return await _context.Shipments
    //            .Include(s => s.Order)
    //            .Include(s => s.Carrier)
    //            .AsNoTracking()
    //            .ToListAsync();
    //    }

    //    public async Task<Shipment?> GetShipmentByIdAsync(int id)
    //    {
    //        return await _context.Shipments
    //            .Include(s => s.Order)
    //            .Include(s => s.Carrier)
    //            .FirstOrDefaultAsync(s => s.ShipmentID == id);
    //    }

    //    public async Task<Shipment> AddShipmentAsync(Shipment shipment)
    //    {
    //        await _context.Shipments.AddAsync(shipment);
    //        await _context.SaveChangesAsync();
    //        return shipment;
    //    }

    //    public async Task UpdateShipmentAsync(Shipment shipment)
    //    {
    //        _context.Shipments.Update(shipment);
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task DeleteShipmentAsync(int id)
    //    {
    //        var shipment = await _context.Shipments.FindAsync(id);
    //        if (shipment != null)
    //        {
    //            _context.Shipments.Remove(shipment);
    //            await _context.SaveChangesAsync();
    //        }
    //    }

    //    public async Task<bool> ShipmentExistsAsync(int id)
    //    {
    //        return await _context.Shipments.AnyAsync(s => s.ShipmentID == id);
    //    }
    //}

    //public class UnitOfWork : IUnitOfWork
    //{
    //    private readonly AppDbContext _context;
    //    public IProductRepo Products { get; }
    //    public IOrderRepo Orders { get; }
    //    public IOrderItemRepo OrderItems { get; }
    //    public IPaymentRepo Payments { get; }
    //    public IShipmentRepo Shipments { get; }

    //    public UnitOfWork(
    //        AppDbContext context,
    //        IProductRepo productRepo,
    //        IOrderRepo orderRepo,
    //        IOrderItemRepo orderItemRepo,
    //        IPaymentRepo paymentRepo,
    //        IShipmentRepo shipmentRepo)
    //    {
    //        _context = context;
    //        Products = productRepo;
    //        Orders = orderRepo;
    //        OrderItems = orderItemRepo;
    //        Payments = paymentRepo;
    //        Shipments = shipmentRepo;
    //    }

    //    public async Task<int> CompleteAsync()
    //    {
    //        return await _context.SaveChangesAsync();
    //    }

    //    public void Dispose()
    //    {
    //        _context.Dispose();
    //    }
    //}
}
