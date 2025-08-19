using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ProductDTOs
{
    public class ProductImageCreateDTO
    {
        [Required, MaxLength(500)]
        public string Url { get; set; } = null!;

        public bool IsPrimary { get; set; }

        [MaxLength(200)]
        public string? AltText { get; set; }

        [Required]
        public int ProductId { get; set; }
    }

    public class ProductImageUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(500)]
        public string? Url { get; set; }

        public bool? IsPrimary { get; set; }

        [MaxLength(200)]
        public string? AltText { get; set; }

        public int? ProductId { get; set; }
    }

    public class ProductImageResponseDTO
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public string AltText { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
