//using DAL_OnlineStore.Entities.Models.OrderModels;
//using DAL_OnlineStore.Entities.Models.ShipmentModels;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAL_OnlineStore.Entities.Models.ShipmentModels
//{
//    public class Shipment
//    {
//        public int ShipmentID { get; set; }
//        public int OrderID { get; set; }
//        public int CarrierID { get; set; }
//        public string? TrackingNumber { get; set; }
//        public DateOnly ShippedDate { get; set; }
//        public DateOnly EstimatedDelivery {  get; set; }
//        public DateOnly ActualDelivery { get; set; }
//        public ShippingStatus status { get; set; } = ShippingStatus.Processing;



//        public Carrier carrier { get; set; } = null!;
//        public Order? order { get; set; } =null!;
//    }

//    public enum ShippingStatus
//    {
//        Processing = 0,
//        OutForDelivery = 1,
//        Delivered = 2,
//        ReturnToSender = 3,
//        OnHold = 4,
//        Delayed = 5,
//        Lost = 6
//    }
//}

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
