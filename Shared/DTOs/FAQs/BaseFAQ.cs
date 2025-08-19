using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.FAQs
{
    // BaseFAQReadDto.cs
    public class BaseFAQReadDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
    }

    // BaseFAQCreateDto.cs
    public class BaseFAQCreateDto
    {
        [Required]
        [StringLength(500)]
        public string Question { get; set; } = null!;

        [Required]
        public string Answer { get; set; } = null!;
    }

    // BaseFAQUpdateDto.cs
    public class BaseFAQUpdateDto
    {
        [StringLength(500)]
        public string? Question { get; set; }

        public string? Answer { get; set; }
    }

    // BaseFAQSimpleDto.cs
    public class BaseFAQSimpleDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
    }
}
