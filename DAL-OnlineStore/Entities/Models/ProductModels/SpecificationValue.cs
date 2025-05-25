using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ProductModels
{
    public class SpecificationValue
    {
        public int Value_id { get; set; }
        public int SpecificationId { get; set; }
        public string? Value { get; set; }   
        public Specification? specification { get; set; }
        public ICollection<ProductSpecification>? ProductSpecifications { get; set; }

    }
}
