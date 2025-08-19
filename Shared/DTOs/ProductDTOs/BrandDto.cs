using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ProductDTOs
{
    public class BrandCreateDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? LogoUrl { get; set; }
    }

    public class BrandUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? LogoUrl { get; set; }
    }

    public class BrandResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public int ProductCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
