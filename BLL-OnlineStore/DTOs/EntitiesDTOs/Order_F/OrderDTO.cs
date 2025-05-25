using BLL_OnlineStore.DTOs.EntitiesDTOs.Payment_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F
{
    /// <summary>
    /// Data Transfer Object for Order entity (for creation and retrieval)
    /// </summary>
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }

        // Snapshot of Address
        public OrderAddressDTO? OrderAddress { get; set; }

        // Payment and Shipment
        public PaymentDTO? Payment { get; set; }
        public ShipmentDTO? Shipment { get; set; }

        // Order Items
        public List<OrderItemDTO>? Items { get; set; }

        // Financial details
        public decimal SubtotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }


        // Notes
        public string? UserNotes { get; set; }
        public string? InternalNotes { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

}

