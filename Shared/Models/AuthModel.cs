namespace Shared.Models
{
    public class AuthModel
    {
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenExpiry { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        // معلومات المستخدم من ApplicationUser
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = null!; 
        public List<string> Roles { get; set; } = [];

        // معلومات شخصية إضافية من Customer
        public string? FullName { get; set; } // اسم العميل
        public DateTime? DateOfBirth { get; set; } // تاريخ ميلاد العميل

        public string Message { get; set; } = string.Empty;

        public int CustomerId { get; set; }
    }
}
