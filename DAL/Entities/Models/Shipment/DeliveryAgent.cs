using DAL.Entities.Identity;
using DAL.Entities.Models.PaymentsModels;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities.Models.Shipment
{
    public class DeliveryAgent : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // اسم المندوب

        [StringLength(11)]
        public string? PhoneNumber { get; set; }

        [StringLength(250)]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true; // هل المندوب نشط

        // إذا كان المندوب مستخدمًا مسجلًا في نظام الهوية الخاص بك
        // يمكنك ربطه بكيان المستخدم (ApplicationUser)
        public string? UserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // IAuditableEntity properties
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = "System";
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;

        // Navigation property for collections made by this agent
        public virtual ICollection<PaymentCollection> PaymentCollections { get; set; } = new List<PaymentCollection>();
    }
}
