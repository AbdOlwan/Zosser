using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    // API/DTOs/ProductCreateUpdateDto.cs
    public class ProductCreateUpdateDto : IValidatableObject
    {
        [Required]
        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }
        public int QuantityInStock { get; set; }

        // غير نصّية فلا تحتاج ترجمة
        public int Brand_ID { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public string Slug { get; set; } = null!;       // https://g.co/gemini/share/e028c67fe505 For Example : classic-cotton-t-shirt-blue-m.
        public string Sku { get; set; } = null!;      // https://g.co/gemini/share/e028c67fe505 For Ex: NIKE-TR-MEN-BLU-M
        // قائمة الترجمات؛ نُلزم وجود ترجمتين (ar + en)
        [Required]
        public IList<ProductTranslationDto> Translations { get; set; } = new List<ProductTranslationDto>();

        // **تأكيد وجود ترجمة عربية وإنجليزية بالضبط**
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var cultures = Translations.Select(t => t.Culture).Distinct().ToList();
            if (!cultures.Contains("ar"))
                yield return new ValidationResult("Arabic translation (culture='ar') is required.",
                                                  new[] { nameof(Translations) });
            if (!cultures.Contains("en"))
                yield return new ValidationResult("English translation (culture='en') is required.",
                                                  new[] { nameof(Translations) });
            if (Translations.Count != cultures.Count)
                yield return new ValidationResult("Duplicate cultures are not allowed.",
                                                  new[] { nameof(Translations) });
        }
    }

}
