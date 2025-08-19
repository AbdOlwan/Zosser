using DAL.Entities.Identity;
using DAL.Entities.Models.ProductModels; // <-- إضافة مهمة
using Shared.DTOs.Order;
using Shared.DTOs.Payments;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Models.OrderModels
{
    public class Order : IAuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        // --- Product Details (Merged from OrderItem) ---
        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } // سعر الوحدة وقت الطلب

        // Product snapshot details
        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string ProductColor { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string ProductSize { get; set; } = null!;


        // --- Financial Details ---

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ShippingCost { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CodFee { get; set; } // رسوم الدفع عند الاستلام

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; } // الإجمالي: Subtotal + ShippingCost + CodFee

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCashback { get; set; } // إجمالي الكاش باك للطلب


        // --- Order Status and Info ---
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public PaymentMethod PaymentMethod { get; set; }


        // --- Shipping Address Properties ---
        [Required]
        [MaxLength(150)]
        public string ContactName { get; set; } = null!;

        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string AddressLine1 { get; set; } = null!;

        [MaxLength(200)]
        public string? AddressLine2 { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string State { get; set; } = null!;

        // --- IAuditableEntity Properties ---
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public DateTime LastModifiedAt { get; set; } 
        public string  LastModifiedBy { get; set; } = string.Empty;
    }
}