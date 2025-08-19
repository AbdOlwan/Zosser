using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.CustomerDtos
{
    /// <summary>
    /// DTO لإنشاء عميل جديد
    /// </summary>
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "الاسم يجب أن يكون بين 3 و 100 حرفاً")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "رقم الهاتف يجب أن يكون 11 رقماً")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$", ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01011111111")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [StringLength(256, ErrorMessage = "البريد الإلكتروني يجب أن يكون أقل من 256 حرفاً")]
        public string? Email { get; set; }

        [StringLength(10, ErrorMessage = "الجنس يجب أن يكون أقل من 10 أحرف")]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO لتحديث بيانات العميل
    /// </summary>
    public class UpdateCustomerDto
    {
        [Required(ErrorMessage = "معرف العميل مطلوب")]
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "الاسم يجب أن يكون بين 3 و 100 حرفاً")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [StringLength(256, ErrorMessage = "البريد الإلكتروني يجب أن يكون أقل من 256 حرفاً")]
        public string? Email { get; set; }

        [StringLength(10, ErrorMessage = "الجنس يجب أن يكون أقل من 10 أحرف")]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }

    /// <summary>
    /// DTO لعرض بيانات العميل
    /// </summary>
    public class CustomerDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public List<string> Roles { get; set; } = [];
    }

    /// <summary>
    /// DTO لعرض بيانات العميل مع تفاصيل محدودة
    /// </summary>
    public class CustomerSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    /// <summary>
    /// DTO لتغيير كلمة المرور
    /// </summary>
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور الجديدة مطلوب")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيدها غير متطابقين")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO لتفعيل/إلغاء تفعيل العميل
    /// </summary>
    public class ToggleCustomerStatusDto
    {
        [Required(ErrorMessage = "معرف العميل مطلوب")]
        public int Id { get; set; }

        [Required(ErrorMessage = "حالة التفعيل مطلوبة")]
        public bool IsActive { get; set; }
    }


}
