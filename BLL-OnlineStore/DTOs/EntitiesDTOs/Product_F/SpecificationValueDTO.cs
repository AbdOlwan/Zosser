using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    public class SpecificationValueDTO
    {
        public int Value_id { get; set; }
        public int SpecificationId { get; set; }

        public string Value { get; set; } = null!;
    }
}
