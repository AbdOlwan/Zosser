using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ProductModels
{
    public class Brand
    {
        [Key] 
        public int Brand_ID { get; set; }

        public string Slug { get; set; } = null!;         // The Same of the brand Name in small letters => nike

        public IList<Product>? Products { get; set; }
        public IList<BrandTranslation> BrandTranslations { get; set; } = [];
    }

    public class BrandTranslation
    {
        public int Id { get; set; }
        public int Brand_ID { get; set; }
        public string Culture { get; set; } = null!;      // "ar" أو "en"
        public string Brand_Name { get; set; } = null!;       // Nike
        public Brand Brand { get; set; } = null!;

    }
}
