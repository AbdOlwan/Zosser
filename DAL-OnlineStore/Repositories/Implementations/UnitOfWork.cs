using DAL_OnlineStore.Context;
using DAL_OnlineStore.Repositories.Interfaces;
using DAL_OnlineStore.Repositories.Interfaces.OrderRepository;
using DAL_OnlineStore.Repositories.Interfaces.PaymentRepository;
using DAL_OnlineStore.Repositories.Interfaces.ShipmentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IProductRepo Products { get; }
        public IOrderRepo Orders { get; }
        public IOrderItemRepo OrderItems { get; }
        public IPaymentRepo Payments { get; }
        public IShipmentRepo Shipments { get; }

        public UnitOfWork(
            AppDbContext context,
            IProductRepo productRepo,
            IOrderRepo orderRepo,
            IOrderItemRepo orderItemRepo,
            IPaymentRepo paymentRepo,
            IShipmentRepo shipmentRepo)
        {
            _context = context;
            Products = productRepo;
            Orders = orderRepo;
            OrderItems = orderItemRepo;
            Payments = paymentRepo;
            Shipments = shipmentRepo;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
