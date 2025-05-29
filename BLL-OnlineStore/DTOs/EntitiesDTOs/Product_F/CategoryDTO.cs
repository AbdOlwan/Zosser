using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Slug { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public List<CategoryTranslationDTO> Translations { get; set; } = [];
    }
    public class CategoryTranslationDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Culture { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
    public class CreateCategoryDTO
    {
        [Required]
        public string Slug { get; set; } = null!;

        [Required]
        public string ArCategoryName { get; set; } = null!;

        [Required]
        public string EnCategoryName { get; set; } = null!;
    }
    public class UpdateCategoryDTO
    {
        public int CategoryId { get; set; }

        [Required]
        public string Slug { get; set; } = null!;

        [Required]
        public string ArCategoryName { get; set; } = null!;

        [Required]
        public string EnCategoryName { get; set; } = null!;
    }

}
