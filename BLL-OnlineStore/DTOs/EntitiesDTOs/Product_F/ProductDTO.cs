using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{


    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Slug { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int QuantityInStock { get; set; }

        public string Brand_Name { get; set; } = null!;
        public string Category_Name { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string MarketingSlogan { get; set; } = null!;
        public List<string> ImageUrls { get; set; } = null!;
    }

    public class CreateProductDTO
    {
        public int TypeId { get; set; }
        public int Brand_ID { get; set; }
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Slug { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Sku { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public decimal DiscountPrice { get; set; }

        [Required]
        public int QuantityInStock { get; set; }

        // Arabic translation
        [Required]
        [MaxLength(255)]
        public string ArProductName { get; set; } = null!;

        [Required]
        public string ArDescription { get; set; } = null!;

        [Required]
        public string ArMarketingSlogan { get; set; } = null!;

        // English translation
        [Required]
        [MaxLength(255)]
        public string EnProductName { get; set; } = null!;

        [Required]
        public string EnDescription { get; set; } = null!;

        [Required]
        public string EnMarketingSlogan { get; set; } = null!;
    }

    // DTO for updating an existing product
    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public int TypeId { get; set; }
        public int Brand_ID { get; set; }
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Slug { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Sku { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public decimal DiscountPrice { get; set; }

        [Required]
        public int QuantityInStock { get; set; }

        // Arabic translation
        [Required]
        [MaxLength(255)]
        public string ArProductName { get; set; } = null!;

        [Required]
        public string ArDescription { get; set; } = null!;

        [Required]
        public string ArMarketingSlogan { get; set; } = null!;

        // English translation
        [Required]
        [MaxLength(255)]
        public string EnProductName { get; set; } = null!;

        [Required]
        public string EnDescription { get; set; } = null!;

        [Required]
        public string EnMarketingSlogan { get; set; } = null!;
    }

    // DTO for product with all translations
    public class ProductWithTranslationsDTO
    {
        public int ProductId { get; set; }
        public int TypeId { get; set; }
        public int Brand_ID { get; set; }
        public int CategoryId { get; set; }
        public string Slug { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ProductTranslationDTO> Translations { get; set; } = new List<ProductTranslationDTO>();
    }

    // DTO for product translations
    public class ProductTranslationDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Culture { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string MarketingSlogan { get; set; } = null!;
    }
}







//public class ProductDTO
//{
//    public int ProductId { get; set; }
//    public int TypeId { get; set; } // خاصية المفتاح الأجنبي
//    public int Brand_ID { get; set; }
//    public int CategoryId { get; set; }

//    public string ProductName { get; set; } = null!;
//    public string Slug { get; set; } = null!;       // https://g.co/gemini/share/e028c67fe505 For Example : classic-cotton-t-shirt-blue-m.
//    public string Description { get; set; } = null!;
//    public string Sku { get; set; } = null!;      // https://g.co/gemini/share/e028c67fe505 For Ex: NIKE-TR-MEN-BLU-M
//    public decimal Price { get; set; }
//    public decimal DiscountPrice { get; set; }
//    public int QuantityInStock { get; set; }
//    public string MarketingSlogan { get; set; } = null!;// الخاصية الجديدة للوصف التسويقي
//    public DateTime CreatedAt { get; set; }
//    public DateTime UpdatedAt { get; set; }
//}


// DTO for creating a new product





//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
//{
//    public class ProductDTO
//    {
//        public int ProductId { get; set; }
//        public int TypeId { get; set; } // خاصية المفتاح الأجنبي
//        public int Brand_ID { get; set; }
//        public int CategoryId { get; set; }

//        public string ProductName { get; set; } = null!;
//        public string Slug { get; set; } = null!;       // https://g.co/gemini/share/e028c67fe505 For Example : classic-cotton-t-shirt-blue-m.
//        public string Description { get; set; } = null!;
//        public string Sku { get; set; } = null!;      // https://g.co/gemini/share/e028c67fe505 For Ex: NIKE-TR-MEN-BLU-M
//        public decimal Price { get; set; }
//        public decimal DiscountPrice { get; set; }
//        public int QuantityInStock { get; set; }
//        public string MarketingSlogan { get; set; } = null!;// الخاصية الجديدة للوصف التسويقي
//        public DateTime CreatedAt { get; set; }
//        public DateTime UpdatedAt { get; set; }
//    }
//}
