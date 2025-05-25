using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.UserDTOs
{
    public class AddRoleDTO
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string RoleName { get; set; } =null!;
    }
}
