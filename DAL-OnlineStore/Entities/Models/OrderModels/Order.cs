
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DAL_OnlineStore.Entities.Models.OrderModels
{
    public class Order
    {

            [Key]
            public int OrderID { get; set; }

            // ─── بيانات العميل والتاريخ ────────────────────────────────
            [Required]
            public int CustomerID { get; set; }
            public Customer? Customer { get; set; }

            [Required]
            public DateTime OrderDate { get; set; } = DateTime.UtcNow;

            [Required]
            public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

            // ─── العنوان الثابت عند الطلب (Snapshot Address) ──────────
            public OrderAddress? OrderAddress { get; set; }

            // ─── تفاصيل الدفع والشحن (يُضافان تلقائيّاً عبر الرسم البياني) ─
            public Payment? Payment { get; set; }
            public Shipment? Shipment { get; set; }

            // ─── العناصر المرتبطة بالطلب ────────────────────────────────
            public ICollection<OrderItem>? Items { get; set; }

            // ─── الحقول المالية ────────────────────────────────────────
            [Required, Column(TypeName = "decimal(18,2)")]
            public decimal SubtotalAmount { get; set; }
            [Column(TypeName = "decimal(18,2)")]
            public decimal TaxAmount { get; set; }
            [Column(TypeName = "decimal(18,2)")]
            public decimal ShippingAmount { get; set; }
            [Column(TypeName = "decimal(18,2)")]
            public decimal DiscountAmount { get; set; }
            [Required, Column(TypeName = "decimal(18,2)")]
            public decimal TotalAmount { get; set; }
            // ─── ملاحظات المستخدم والإدارة الداخلية ──────────────────
            public string? UserNotes { get; set; }
            public string? InternalNotes { get; set; }

            // ─── حقول التدقيق (Audit) ─────────────────────────────────
            [Required]
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public string? CreatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public string? UpdatedBy { get; set; }
            public bool IsDeleted { get; set; } = false;
        }

    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4,
        Refunded = 5
    }
}