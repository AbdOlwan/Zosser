
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ShipmentModels
{

    public class Shipment
    {
        [Key]
        public int ShipmentID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public Order Order { get; set; } = null!;

        public int CarrierID { get; set; }
        public Carrier? Carrier { get; set; }
        public string? TrackingNumber { get; set; }

        public DateTime? ShippedDate { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }

        public ShippingStatus Status { get; set; } = ShippingStatus.Processing;
    }

    public enum ShippingStatus
    {
        Processing = 0,
        OutForDelivery = 1,
        Delivered = 2,
        ReturnToSender = 3,
        OnHold = 4,
        Delayed = 5,
        Lost = 6
    }
}
