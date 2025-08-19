using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.IdentityDtos
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "الاسم يجب أن يكون بين 3 و 100 حرفاً")]
        public string FullName { get; set; } = string.Empty;


        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "رقم الهاتف يجب أن يكون 11 رقماً")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$", ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01011111111")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


    }
}
