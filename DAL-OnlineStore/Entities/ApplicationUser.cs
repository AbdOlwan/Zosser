using DAL_OnlineStore.Entities.Models.People;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities
{

    public class ApplicationUser : IdentityUser
    {


        public int PersonId { get; set; }
       // public string UserName { get; set; } = null!;
        public Person? Person { get; set; }
    }


}
