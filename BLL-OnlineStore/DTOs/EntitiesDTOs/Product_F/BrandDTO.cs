using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    public class BrandDTO
    {
        public int Brand_ID { get; set; }
        public string Slug { get; set; } = null!;

        // ترجمة واحدة فقط حسب اللغة الحالية (للعرض)
        public string Brand_Name { get; set; } = null!;

        // كل الترجمات (في حالة الإدارة أو التعديل)
        public List<BrandTranslationDTO> Translations { get; set; } = [];
    }


    public class BrandTranslationDTO
    {
        public int Id { get; set; }
        public int Brand_ID { get; set; }
        public string Culture { get; set; } = null!;
        public string Brand_Name { get; set; } = null!;
    }


    public class CreateBrandDTO
    {
        [Required]
        public string Slug { get; set; } = null!;

        // Arabic translation
        [Required]
        [MaxLength(255)]
        public string ArBrandName { get; set; } = null!;

        // English translation
        [Required]
        [MaxLength(255)]
        public string EnBrandName { get; set; } = null!;
    }
    public class UpdateBrandDTO
    {
        public int Brand_ID { get; set; }

        [Required]
        public string Slug { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string ArBrandName { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string EnBrandName { get; set; } = null!;
    }

}
