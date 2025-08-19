using Shared.Interfaces;

namespace DAL.Entities.Models.ProductModels
{
    public class Brand : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? LogoUrl { get; set; }

        // العلاقات
        public ICollection<Product> Products { get; set; } = new List<Product>();

        // التتبع
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }
}
