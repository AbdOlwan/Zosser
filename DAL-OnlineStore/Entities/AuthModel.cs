using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities
{
    public class AuthModel
    {
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public DateTime Expiry { get; set; }
        public List<string> Roles { get; set; } = new();
        public string Username { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$", ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01132563254")]
        public string PhoneNumber { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
    }


}
