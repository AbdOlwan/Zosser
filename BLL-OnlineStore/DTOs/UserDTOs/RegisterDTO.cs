using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.UserDTOs
{


    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; } = null!;    // display name

        [Required]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$",ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01132563254")]
        public string PhoneNumber { get; set; } = null!; // login identity

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        // Optional Input Data
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public char? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
    }

    //public class RegisterDTO
    //{
    //    public string UserName { get; set; }
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Phone { get; set; }
    //    public bool Gender { get; set; }
    //    public DateOnly DateOfBirth { get; set; }
    //    public bool IsActive { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    // ... أي خصائص أخرى مطلوبة للتسجيل
    //}


}
