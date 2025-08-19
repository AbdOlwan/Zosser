using Shared.DTOs.ProductDTOs.General;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ProductDTOs
{


    public class ProductCreateDTO
    {
        [Required, MaxLength(255)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string Description { get; set; } = null!;

        [Required, Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cashback { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public int StockQuantity { get; set; }


        // Specifications

        [Required, MaxLength(100)]
        public string Material { get; set; } = null!;

        [MaxLength(50)]
        public string? FitType { get; set; }

        [MaxLength(20)]
        public string? TargetGender { get; set; }

        [Required, MaxLength(50)]
        public string Color { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Size { get; set; } = null!;

        [MaxLength(500)]
        public string? CareInstructions { get; set; }

        [MaxLength(100)]
        public string? CountryOfOrigin { get; set; }

        // Navigations

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int BrandId { get; set; }

       // public List<ProductImageCreateDTO> Images { get; set; } = new();
    }

    public class ProductUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Cashback { get; set; }

        [Range(0, double.MaxValue)]
        public int StockQuantity { get; set; }


        [MaxLength(100)]
        public string? Material { get; set; }

        [MaxLength(50)]
        public string? FitType { get; set; }

        [MaxLength(20)]
        public string? TargetGender { get; set; }

        [MaxLength(50)]
        public string? Color { get; set; }

        [MaxLength(50)]
        public string? Size { get; set; }

        [MaxLength(500)]
        public string? CareInstructions { get; set; }

        [MaxLength(100)]
        public string? CountryOfOrigin { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }

       // public List<ProductImageUpdateDTO>? Images { get; set; }
    }


    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Cashback { get; set; }
        public string Material { get; set; } = null!;
        public string? FitType { get; set; }
        public string? TargetGender { get; set; }
        public string Color { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string? CareInstructions { get; set; }
        public string? CountryOfOrigin { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public List<ProductImageResponseDTO> Images { get; set; } = new();

        public DateTime CreatedAt { get; set; }
    }

    // موجود مسبقاً (يستخدم للبطاقات)
    public class ProductCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal CashbackAmount { get; set; }
        public decimal Rating { get; set; } // متوسط التقييم
        public string PrimaryImageUrl { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public int ReviewCount { get; set; } // عدد التقييمات
    }
}