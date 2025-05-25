using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;

namespace DAL_OnlineStore.Repositories.Interfaces
{
    //public interface IProductRepo
    //{
    //    Task<IEnumerable<Product>> GetAllProductsAsync(string culture);
    //    Task<Product?> GetProductByIdAsync(int id, string? culture);
    //    Task<Product?> GetProductBySlugAsync(string slug, string culture);
    //    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId, string culture);
    //    Task<IEnumerable<Product>> GetProductsByBrandIdAsync(int brandId, string culture);
    //    Task<IEnumerable<Product>> GetProductsByTypeIdAsync(int typeId, string culture);
    //    Task<IEnumerable<Product>> FindProductsAsync(Expression<Func<Product, bool>> predicate, string culture);
    //    Task<Product> AddProductAsync(Product product);
    //    Task UpdateProductAsync(Product product);
    //    Task DeleteProductAsync(int id);
    //    Task<bool> ProductExistsAsync(int id);
    //    Task<bool> ProductExistsBySlugAsync(string slug);
    //}

    //public interface IOrderRepo
    //{
    //    Task<IEnumerable<Order>> GetAllOrdersAsync();
    //    Task<Order?> GetOrderByIdAsync(int id);
    //    Task<Order> AddOrderAsync(Order order);
    //    Task UpdateOrderAsync(Order order);
    //    Task DeleteOrderAsync(int id);
    //    Task<bool> OrderExistsAsync(int id);
    //    Task<IEnumerable<Order>> FindOrdersAsync(Expression<Func<Order, bool>> predicate);
    //}

    //public interface IOrderItemRepo
    //{
    //    Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
    //    Task<OrderItem?> GetOrderItemByIdAsync(int id);
    //    Task<OrderItem> AddOrderItemAsync(OrderItem item);
    //    Task UpdateOrderItemAsync(OrderItem item);
    //    Task DeleteOrderItemAsync(int id);
    //    Task<bool> OrderItemExistsAsync(int id);
    //}

    //public interface IPaymentRepo
    //{
    //    Task<IEnumerable<Payment>> GetAllPaymentsAsync();
    //    Task<Payment?> GetPaymentByIdAsync(int id);
    //    Task<Payment> AddPaymentAsync(Payment payment);
    //    Task UpdatePaymentAsync(Payment payment);
    //    Task DeletePaymentAsync(int id);
    //    Task<bool> PaymentExistsAsync(int id);
    //}

    //public interface IShipmentRepo
    //{
    //    Task<IEnumerable<Shipment>> GetAllShipmentsAsync();
    //    Task<Shipment?> GetShipmentByIdAsync(int id);
    //    Task<Shipment> AddShipmentAsync(Shipment shipment);
    //    Task UpdateShipmentAsync(Shipment shipment);
    //    Task DeleteShipmentAsync(int id);
    //    Task<bool> ShipmentExistsAsync(int id);
    //}

    //public interface IUnitOfWork : IDisposable
    //{
    //    IProductRepo Products { get; }
    //    IOrderRepo Orders { get; }
    //    IOrderItemRepo OrderItems { get; }
    //    IPaymentRepo Payments { get; }
    //    IShipmentRepo Shipments { get; }
    //    Task<int> CompleteAsync();
    //}
}
