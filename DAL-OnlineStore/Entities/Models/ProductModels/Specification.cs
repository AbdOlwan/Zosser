using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ProductModels
{


    public class Specification
    {
        public int SpecificationId { get; set; }
        public ICollection<SpecificationValue> SpecificationValues { get; set; } = [];
        public ICollection<SpecificationTranslation> specificationTranslations { get; set; } = [];

    }

    /*
 هذا الجدول سيحتوي على أنواع المواصفات المختلفة التي يمكن أن تنطبق على المنتجات
(مثل "اللون"، "المقاس"، "الخامة"، "تعليمات العناية").
هذا الجدول سيجعل قاعدة البيانات أكثر مرونة وقابلية للتوسع.
 */

    public class SpecificationTranslation
    {
        public int Id { get; set; }
        public int SpecificationId { get; set; }
        public string Culture { get; set; } = null!;      // "ar" أو "en"
        public string Specification_Name { get; set; } = null!;
        public Specification specification {  get; set; }   =null!;
    }
}
