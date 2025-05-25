using DAL_OnlineStore.Repositories.Interfaces.OrderRepository;
using DAL_OnlineStore.Repositories.Interfaces.PaymentRepository;
using DAL_OnlineStore.Repositories.Interfaces.ShipmentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepo Products { get; }
        IOrderRepo Orders { get; }
        IOrderItemRepo OrderItems { get; }
        IPaymentRepo Payments { get; }
        IShipmentRepo Shipments { get; }
        Task<int> CompleteAsync();
    }
}
