using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ProductModels
{
    public class ProductSpecification
    {
       // [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

       // [ForeignKey("SpecificationValue")]
        public int ValueId { get; set; }
        public SpecificationValue SpecificationValue { get; set; } = null!;
    }
}
