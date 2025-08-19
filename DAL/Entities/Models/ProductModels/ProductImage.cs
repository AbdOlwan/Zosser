using Shared.Interfaces;

namespace DAL.Entities.Models.ProductModels
{
    public class ProductImage : IAuditableEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string Url { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public string AltText { get; set; } = string.Empty;

        // التتبع
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }

}
