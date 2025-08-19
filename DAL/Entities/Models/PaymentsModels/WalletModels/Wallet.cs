using DAL.Entities.Identity;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Models.PaymentsModels.WalletModels
{
    public class Wallet : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; } = 0.00m; // الرصيد الحالي للكاش باك

        [Required]
        public int CustomerId { get; set; } // Foreign Key to Customer
        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
        public virtual ICollection<WithdrawalRequest> WithdrawalRequests { get; set; } = new List<WithdrawalRequest>();

        // IAuditableEntity properties
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }
}
