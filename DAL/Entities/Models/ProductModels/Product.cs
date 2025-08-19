using DAL.Entities.Models.FaqModels;
using DAL.Entities.Models.OrderModels;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;


namespace DAL.Entities.Models.ProductModels
{

    public class Product : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Cashback { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        // خصائص الملابس
        public string Material { get; set; } = null!;
        public string? CareInstructions { get; set; }
        public string? CountryOfOrigin { get; set; }
        public string? FitType { get; set; } // مثل: Slim, Regular, Oversized
        public string? TargetGender { get; set; } // رجالي، نسائي، أطفال

        // لون واحد وحجم واحد فقط
        public string Color { get; set; } = null!;       // لون واحد
        public string Size { get; set; } = null!;        // مقاس واحد

        // العلاقات
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductFAQ>? FAQs { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        // التتبع
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }

}










