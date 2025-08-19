using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Dashboard
{
    public class UserSummaryDto
    {
        public string UserId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<string> Roles { get; set; } = [];
    }

    // DTO لعرض التفاصيل الكاملة لمستخدم واحد
    public class UserDetailsDto : UserSummaryDto
    {
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }

    // DTO لتحديث حالة المستخدم (تفعيل/تعطيل)
    public class UpdateUserStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }

    // DTO لإدارة أدوار المستخدم
    public class ManageUserRolesDto
    {
        [Required]
        public string RoleName { get; set; } = null!;
    }

    public class CreateStaffUserDto
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "الصلاحية مطلوبة")]
        // يمكنك إضافة Validation للتأكد أن الصلاحية هي واحدة من الصلاحيات المسموحة
        public string RoleName { get; set; } = string.Empty;
    }
}
