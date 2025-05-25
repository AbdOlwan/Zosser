using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs
{
    //public class OrderDTO
    //{
    //    public int OrderID { get; set; }
    //    public int CustomerID { get; set; }
    //    public int OrderAddressID { get; set; }
    //    public DateTime OrderDate { get; set; }
    //    public OrderStatus OrderStatus { get; set; }
    //    public decimal TotalAmount { get; set; }
    //    public IEnumerable<OrderItemDTO>? Items { get; set; }
    //    public PaymentDTO? Payment { get; set; }
    //    public ShipmentDTO? Shipment { get; set; }
    //}

    //public class OrderAddressDTO
    //{
    //    public int OrderAddressID { get; set; }
    //    public string AddressLine1 { get; set; } = null!;
    //    public string? AddressLine2 { get; set; }
    //    public string City { get; set; } = null!;
    //    public string State { get; set; } = null!;
    //    public string? PostalCode { get; set; }
    //    public string Country { get; set; } = null!;
    //}

    //public class OrderItemDTO
    //{
    //    public int OrderItemID { get; set; }
    //    public int ProductID { get; set; }
    //    public int Quantity { get; set; }
    //    public decimal UnitPrice { get; set; }
    //    public decimal Subtotal { get; set; }
    //}

    //public class PaymentDTO
    //{
    //    public int PaymentID { get; set; }
    //    public int OrderID { get; set; }
    //    public string? TransactionID { get; set; }
    //    public decimal Amount { get; set; }
    //    public DateTime PaymentDate { get; set; }
    //    public PaymentStatus Status { get; set; }
    //    public PaymentMethod Method { get; set; }
    //}

    //public class ShipmentDTO
    //{
    //    public int ShipmentID { get; set; }
    //    public int OrderID { get; set; }
    //    public int CarrierID { get; set; }
    //    public string? TrackingNumber { get; set; }
    //    public DateTime? ShippedDate { get; set; }
    //    public DateTime? EstimatedDelivery { get; set; }
    //    public DateTime? ActualDelivery { get; set; }
    //    public ShippingStatus Status { get; set; }
    //}

    //public class ProductDTO
    //{
    //    public int ProductId { get; set; }
    //    public string Name { get; set; } = null!;
    //    public string? Description { get; set; }
    //    public decimal Price { get; set; }
    //    public int QuantityInStock { get; set; }
    //}
}
