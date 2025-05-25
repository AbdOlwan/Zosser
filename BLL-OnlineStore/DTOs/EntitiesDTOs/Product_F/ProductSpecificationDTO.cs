using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    public class ProductSpecificationDTO
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("SpecificationValue")]
        public int ValueId { get; set; }
    }
}
