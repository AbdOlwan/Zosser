using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.People_F
{
    public class PersonDTO
    {
        public int PersonId { get; set; }
        [Required]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$",ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01000000001")]
        public string PhoneNumber { get; set; } = null!;

        // Optional Input Data
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public char? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
