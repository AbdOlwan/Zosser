using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    public class CategoryDTO
    {
        public int CategoryID { get; set; }
        public string Category_Name { get; set; } = null!;

        public string Slug { get; set; } = null!;  // mens-clothing

    }
}
