using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    public class ProductTranslationDto
    {
        [Required]
        [RegularExpression("^(ar|en)$", ErrorMessage = "Culture must be 'ar' or 'en'.")]
        public string Culture { get; set; } = null!;  // "ar" أو "en"

        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; } = null!;

        public string Description { get; set; } = null!;
        public string MarketingSlogan { get; set; } = null!; // الخاصية الجديدة للوصف التسويقي
    }
}
