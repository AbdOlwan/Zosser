using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Payments.Wallets
{
    //==================== Wallet DTOs ====================

    /// <summary>
    /// DTO لعرض تفاصيل المحفظة الكاملة للعميل، بما في ذلك الرصيد وسجل الحركات.
    /// </summary>
    public class WalletResponseDto
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<WalletTransactionResponseDto> Transactions { get; set; } = [];
    }


    //==================== Wallet Transaction DTOs ====================

    /// <summary>
    /// DTO لعرض تفاصيل حركة مالية واحدة (إيداع أو سحب).
    /// </summary>
    public class WalletTransactionResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty; // "Deposit", "Withdrawal", "Purchase"
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }


    //==================== Withdrawal Request DTOs ====================

    /// <summary>
    /// DTO يستخدمه العميل لإنشاء طلب سحب جديد.
    /// </summary>
    public class WithdrawalRequestCreateDto
    {
        [Required]
        [Range(1.00, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ المراد سحبه أكبر من صفر.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "رقم المحفظة الإلكترونية مطلوب.")]
        [StringLength(20, MinimumLength = 11, ErrorMessage = "الرقم غير صالح.")]
        public string EWalletNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الواتساب مطلوب.")]
        [StringLength(20, MinimumLength = 11, ErrorMessage = "الرقم غير صالح.")]
        public string WhatsAppNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO يستخدمه المسؤول لتحديث حالة طلب السحب.
    /// </summary>
    public class WithdrawalRequestUpdateDto
    {
        [Required]
        public int Status { get; set; } // سيتم تحويله إلى WithdrawalStatus enum في طبقة BLL

        public string? AdminNotes { get; set; }
    }

    /// <summary>
    /// DTO لعرض التفاصيل الكاملة لطلب سحب، مناسب للعميل والمسؤول.
    /// </summary>
    public class WithdrawalRequestResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string EWalletNumber { get; set; } = string.Empty;
        public string WhatsAppNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // "Pending", "Processed", etc.
        public DateTime RequestDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string? AdminNotes { get; set; }
        public string? ProcessedByUserName { get; set; }
    }
    public enum WithdrawalStatus
    {
        Pending,   // قيد المراجعة
        Approved,  // تمت الموافقة (في انتظار التحويل)
        Processed, // تمت المعالجة (تم التحويل)
        Rejected,  // مرفوض
        Cancelled  // تم الإلغاء بواسطة العميل
    }
    public enum TransactionType
    {
        Deposit,      // إيداع (إضافة كاش باك)
        Withdrawal,   // سحب
        Purchase      // استخدام في الشراء
    }
    public class CashbackBatchItemDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; } 
        [Required]
        [Range(0.01, double.MaxValue)] 
        public decimal Amount { get; set; }
        public string? Description { get; set; } = "Cashback credit";
        public string? ExternalReferenceId { get; set; }
        public int? OrderId { get; set; }
    }
}
