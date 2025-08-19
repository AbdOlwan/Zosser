using DAL.Entities.Models.OrderModels;
using Shared.DTOs.Payments;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Models.PaymentsModels
{


    public class Payment : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        // علاقة One-to-One: كل دفعة مرتبطة بطلب واحد فقط
        public virtual Order Order { get; set; } = null!;

        public string? TransactionId { get; set; } // معرف العملية من مزود خدمة الدفع (للمدفوعات الإلكترونية)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // المبلغ المدفوع

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public PaymentMethod Method { get; set; } = PaymentMethod.CashOnDelivery;

        // خصائص IAuditableEntity
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;

        // Navigation property for PaymentCollection (optional, only for CashOnDelivery)
        // كل دفعة يمكن أن يكون لها سجل تحصيل واحد فقط
        public virtual PaymentCollection? PaymentCollection { get; set; }
    }
}
