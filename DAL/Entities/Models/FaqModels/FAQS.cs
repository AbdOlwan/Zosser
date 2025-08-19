using DAL.Entities.Models.ProductModels;
using Shared.Interfaces;

namespace DAL.Entities.Models.FaqModels
{
    // 1. إنشاء فئة أساسية للأسئلة الشائعة (لإدارة الحقول المشتركة)
    public abstract class BaseFAQ : IAuditableEntity
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;

        // خصائص التتبع
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
    }

    // 2. FAQs عامة للموقع
    public class SiteFAQ : BaseFAQ
    {
        // إضافة حقول خاصة هنا مستقبلاً (مثل التصنيفات)
    }

    // 3. FAQs خاصة بالمنتج
    public class ProductFAQ : BaseFAQ
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
