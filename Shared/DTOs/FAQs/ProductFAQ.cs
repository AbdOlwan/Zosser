using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.FAQs
{
    public class ProductFAQReadDto : BaseFAQReadDto
    {
        public int ProductId { get; set; }
        //public ProductSimpleDto Product { get; set; } = null!;
    }

    // ProductFAQCreateDto.cs
    public class ProductFAQCreateDto : BaseFAQCreateDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
    }

    // ProductFAQUpdateDto.cs
    public class ProductFAQUpdateDto : BaseFAQUpdateDto
    {
        [Range(1, int.MaxValue)]
        public int? ProductId { get; set; }
    }

    // ProductFAQSimpleDto.cs
    public class ProductFAQSimpleDto : BaseFAQSimpleDto
    {
        public int ProductId { get; set; }
    }
}
