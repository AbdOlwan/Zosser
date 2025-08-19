using DAL.Entities.Identity;
using Shared.DTOs.Payments.Wallets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Models.PaymentsModels.WalletModels
{


    public class WithdrawalRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; } // المبلغ المطلوب سحبه

        [Required]
        [MaxLength(20)]
        public string EWalletNumber { get; set; } = string.Empty; // رقم المحفظة (فودافون كاش، الخ)

        [Required]
        [MaxLength(20)]
        public string WhatsAppNumber { get; set; } = string.Empty; // رقم الواتساب لإرسال الإثبات

        public WithdrawalStatus Status { get; set; } = WithdrawalStatus.Pending;

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdateDate { get; set; }

        // بيانات المسؤول الذي قام بمعالجة الطلب
        public string? ProcessedByUserId { get; set; } // ID of the admin user
        public virtual ApplicationUser? ProcessedByUser { get; set; }

        [MaxLength(500)]
        public string? AdminNotes { get; set; } // ملاحظات من المسؤول (مثل سبب الرفض)
    }
}
