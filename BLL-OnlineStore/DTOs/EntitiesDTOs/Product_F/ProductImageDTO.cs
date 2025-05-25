using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    public class ProductImageDTO
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string ImageURL { get; set; } = null!;
        public bool IsPrimary { get; set; }
    }
}
