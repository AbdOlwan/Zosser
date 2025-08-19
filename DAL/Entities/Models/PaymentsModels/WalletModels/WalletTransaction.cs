using Shared.DTOs.Payments.Wallets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Models.PaymentsModels.WalletModels
{


    public class WalletTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; } // نوع العملية

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty; // وصف العملية، مثل: "كاش باك من الطلب رقم 123"

        // مفتاح خارجي اختياري لربط الحركة بالكيان المسبب لها
        public int? SourceId { get; set; } // يمكن أن يكون OrderItemId, WithdrawalRequestId, etc.
        public string? SourceType { get; set; } // "OrderItem", "WithdrawalRequest"
    }
}
