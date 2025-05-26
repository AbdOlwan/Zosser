using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.People
{


        public class Person
        {
         public int PersonId { get; set; }    // PK
        [Required]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$",ErrorMessage = "الرجاء إدخال رقم هاتف مصري صالح مثلاً 01011111111")]
        public string PhoneNumber { get; set; } = null!;
        public DateTime CreatedAt { get; set; }    // audit

        // Optional Input Data
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public char? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;



        // navigation
        public ApplicationUser? User { get; set; }
            public Customer? Customer { get; set; }
        }

    }



