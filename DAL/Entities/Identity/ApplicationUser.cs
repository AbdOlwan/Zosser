using DAL.Entities.Models.Shipment;
using Microsoft.AspNetCore.Identity;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities.Identity
{
    public class ApplicationUser : IdentityUser, IAuditableEntity
    {
        // خصائص إضافية لـ ApplicationUser

        // تم تغيير نوع PhoneNumber إلى string? ليتوافق مع IdentityUser
        // يتم فرض حقل الهاتف إلزاميًا عن طريق سمة [Required]
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "رقم الهاتف يجب أن يكون 11 رقماً")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$", ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01011111111")]
        public override string? PhoneNumber { get; set; } // // تم تغييرها من string إلى string? لحل التحذير

        public string Name { get; set; } = string.Empty; //

        public string? Gender { get; set; } // مثال: ذكر / أنثى

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")] //
        [StringLength(256, ErrorMessage = "البريد الإلكتروني طويل جداً")] //
        public override string? Email { get; set; } // override

        public DateTime? DateOfBirth { get; set; } // تاريخ ميلاد المستخدم

        public bool IsActive { get; set; } = true; // هل الحساب نشط؟ (مطلوب)
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow; // تاريخ تسجيل الحساب (مطلوب)

        // خصائص Auditing (من IAuditableEntity)
        public DateTime CreatedAt { get; set; } // تاريخ إنشاء السجل (مطلوب)
        public string CreatedBy { get; set; } = "System"; // هوية من أنشأ السجل، قيمة افتراضية "System" ستُعدّل بـ ICurrentUserService
        public DateTime LastModifiedAt { get; set; } // تاريخ آخر تعديل على السجل (اختياري)
        public string LastModifiedBy { get; set; } = string.Empty; // هوية من قام بآخر تعديل (اختياري)

        public Customer? Customer { get; set; } //
        public DeliveryAgent? DeliveryAgent { get; set; }
    }
}