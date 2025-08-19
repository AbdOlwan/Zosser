using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ProductDTOs.General
{


    // لإنشاء فئة جديدة
    public class CategoryCreateDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? IconUrl { get; set; }
    }

    public class CategoryUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? IconUrl { get; set; }
    }

    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? IconUrl { get; set; }
        public int ProductCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
