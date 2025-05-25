using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ProductModels
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Slug { get; set; } = null!;  // mens-clothing
        public IList<Product> Products { get; set; } = [];
        public IList<CategoryTranslation> Translations { get; set; } = [];
    }

    public class CategoryTranslation
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public string Culture { get; set; } = null!;      // "ar" أو "en"
        public string Category_Name { get; set; } = null!;
        public Category? category { get; set; }  

    }
}
