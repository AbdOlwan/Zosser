using DAL.Entities.Models.Shipment;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Models.PaymentsModels
{
    public class PaymentCollection : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PaymentId { get; set; } // المفتاح الخارجي لربطه بـ Payment (علاقة One-to-One)
        public virtual Payment Payment { get; set; } = null!;

        [Required]
        public int DeliveryAgentId { get; set; } // المفتاح الخارجي للمندوب الذي قام بالتحصيل
        public virtual DeliveryAgent DeliveryAgent { get; set; } = null!;

        public DateTime CollectedAt { get; set; } = DateTime.UtcNow; // تاريخ ووقت التحصيل الفعلي

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // المبلغ الذي تم تحصيله فعليًا

        // من قام بالتحقق من عملية التحصيل (مثلا: مسؤول مالي - المدير)
        // استخدم string إذا كان Id لـ ApplicationUser هو string
        public string? VerifiedByUserId { get; set; }
        // public virtual ApplicationUser? VerifiedByUser { get; set; } // استبدل بكيان المستخدم الفعلي الخاص بك

        public DateTime? VerifiedAt { get; set; } // وقت التحقق من قبل المدير

        // خصائص IAuditableEntity
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }
}
