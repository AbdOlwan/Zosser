using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.IdentityDtos
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "رقم الهاتف يجب أن يكون 11 رقماً")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$", ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01011111111")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
