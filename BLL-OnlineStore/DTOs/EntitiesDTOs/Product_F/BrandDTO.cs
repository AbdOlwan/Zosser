using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    public class BrandDTO
    {
        public int Brand_ID { get; set; }
        public string Brand_Name { get; set; } = null!;      // Nike

        public string Slug { get; set; } = null!;        // The Same of the brand Name in small letters => nike

    }
}
