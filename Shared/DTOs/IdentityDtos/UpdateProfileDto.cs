using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.IdentityDtos
{
    public class UpdateProfileDto
    {
        [Required(ErrorMessage = "معرف المستخدم مطلوب")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 100 حرف")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(256, ErrorMessage = "البريد الإلكتروني طويل جداً")]
        public string? Email { get; set; }

        [StringLength(10, ErrorMessage = "الجنس لا يمكن أن يكون أكثر من 10 أحرف")]
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
