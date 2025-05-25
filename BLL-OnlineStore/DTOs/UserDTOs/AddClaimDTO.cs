using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.UserDTOs
{
    public class AddClaimDTO
    {
        [Required]
        public string userId { get; set; } = null!;

        [Required]
        public string claimType { get; set; } = null!;

        [Required]
        public string claimValue { get; set; }=null!;
    }
}
