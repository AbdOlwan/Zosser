using DAL.Entities.Models.OrderModels;
using DAL.Entities.Models.PaymentsModels;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using Shared.Interfaces;

namespace DAL.Entities.Identity
{
    public class Customer : IAuditableEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        // خصائص Auditing (من IAuditableEntity)
        public DateTime CreatedAt { get; set; } // تاريخ إنشاء السجل (مطلوب)
        public string CreatedBy { get; set; } = "System"; // هوية من أنشأ السجل
        public DateTime LastModifiedAt { get; set; } // تاريخ آخر تعديل على السجل (اختياري)
        public string LastModifiedBy { get; set; }   = string.Empty; // هوية من قام بآخر تعديل (اختياري)
        public ApplicationUser User { get; set; } = null!; // لا يمكن أن يكون null لأنه يجب أن يرتبط بمستخدم
        public virtual Wallet Wallet { get; set; } = null!; // لا يمكن أن يكون null لأنه يجب أن يرتبط بمحفظة
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
