using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.IdentityDtos
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "الـ Token مطلوب")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "الـ Refresh Token مطلوب")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
