using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.ReviewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ProductModels
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int Brand_ID { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; } 

        [MaxLength(255)]                             // Slugلو عايز تعرف ايه هو ال 
        public string Slug { get; set; } = null!;      // https://g.co/gemini/share/e028c67fe505 For Example : classic-cotton-t-shirt-blue-m.
        [MaxLength(50)]                                // Skuلو عايز تعرف ايه هو ال
        public string Sku { get; set; } = null!;          // https://g.co/gemini/share/e028c67fe505 For Ex: NIKE-TR-MEN-BLU-M
        [Precision(10, 2)]
        public decimal Price { get; set; }
        [Precision(10, 2)]
        public decimal DiscountPrice { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }



        public Category? category { get; set; }
        public ProductType? type { get; set; }
        public Brand? brand { get; set; }
        public IList<OrderItem>? Items { get; set; } 
        public IList<ProductImage>? Images { get; set; }
        public IList<Review>? reviews { get; set; }
        public IList<CartItem>? CartItems { get; set; }
        public IList<ProductSpecification>? productSpecifications { get; set; }
        public IList<ProductTranslation> productTranslations { get; set; } = null!;
    }


    public class ProductTranslation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Culture { get; set; } = null!;      // "ar" أو "en"
        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string MarketingSlogan { get; set; } = null!; // الخاصية الجديدة للوصف التسويقي

        public Product Product { get; set; } = null!;
    }

}
