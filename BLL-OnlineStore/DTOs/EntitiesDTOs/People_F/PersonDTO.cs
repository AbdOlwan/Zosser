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
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$",
 ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01132563254")]
        public string PhoneNumber { get; set; } = null!;
    }
}
