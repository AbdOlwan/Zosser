using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Implementations.ProductRepository;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.ProductBusServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICultureService _culture;

        public CategoryServices(
            ICategoryRepo repo,
            IMapper mapper, // تم تصحيح الإملاء
            ICultureService culture)
        {
            _repo = repo;
            _mapper = mapper;
            _culture = culture;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategorys(string culture = "ar")
        {
            var categories = await _repo.GetAllCategoriesWithTranslationsAsync(culture);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id, string culture = "ar")
        {
            var category = await _repo.GetCategoryByIdAsync(id, includeTranslations: true);
            if (category == null)
                return null;

            // إنشاء CategoryDTO جديد
            var categoryDTO = new CategoryDTO
            {
                CategoryId = category.CategoryID,
                Slug = category.Slug,
                Translations = new List<CategoryTranslationDTO>()
            };

            // التعامل مع الترجمات
            if (category.Translations?.Any() == true)
            {
                // Map all translations
                categoryDTO.Translations = category.Translations.Select(t => new CategoryTranslationDTO
                {
                    Id = t.Id,
                    CategoryId = t.CategoryID,
                    Culture = t.Culture,
                    CategoryName = t.Category_Name
                }).ToList();

                // تحديد اسم الفئة حسب الثقافة المطلوبة
                var currentTranslation = category.Translations
                    .FirstOrDefault(t => t.Culture.ToLower() == culture.ToLower());

                categoryDTO.CategoryName = currentTranslation?.Category_Name ??
                    category.Translations.First().Category_Name; // fallback للترجمة الأولى
            }
            else
            {
                // لو مفيش ترجمات خالص
                categoryDTO.CategoryName = string.Empty;
            }

            return categoryDTO;
        }

        public async Task<CategoryDTO?> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO)
        {
            try
            {
                // Map base Category data
                var category = _mapper.Map<Category>(createCategoryDTO);

                // Create translations for Arabic and English
                category.Translations = new List<CategoryTranslation>
                {
                    new CategoryTranslation
                    {
                        Culture = "ar",
                        Category_Name = createCategoryDTO.ArCategoryName
                    },
                    new CategoryTranslation
                    {
                        Culture = "en",
                        Category_Name = createCategoryDTO.EnCategoryName
                    }
                };

                // Save to database
                var createdCategory = await _repo.AddCategoryAsync(category);

                // Return the created category with default culture
                return await GetCategoryByIdAsync(createdCategory.CategoryID, "ar");
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException("Error creating category", ex);
            }
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO)
        {
            // Get existing Category with all translations
            var category = await _repo.GetCategoryByIdAsync(updateCategoryDTO.CategoryId, includeTranslations: true);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {updateCategoryDTO.CategoryId} not found");

            // Update base Category properties (slug, etc.)
            category.Slug = updateCategoryDTO.Slug ?? category.Slug;

            // تأكد إن الـ Translations مش null
            category.Translations ??= new List<CategoryTranslation>();

            // Update Arabic translation
            var arTranslation = category.Translations.FirstOrDefault(t => t.Culture == "ar");
            if (arTranslation != null)
            {
                arTranslation.Category_Name = updateCategoryDTO.ArCategoryName;
            }
            else
            {
                category.Translations.Add(new CategoryTranslation
                {
                    CategoryID = category.CategoryID,
                    Culture = "ar",
                    Category_Name = updateCategoryDTO.ArCategoryName
                });
            }

            // Update English translation
            var enTranslation = category.Translations.FirstOrDefault(t => t.Culture == "en");
            if (enTranslation != null)
            {
                enTranslation.Category_Name = updateCategoryDTO.EnCategoryName;
            }
            else
            {
                category.Translations.Add(new CategoryTranslation
                {
                    CategoryID = category.CategoryID,
                    Culture = "en",
                    Category_Name = updateCategoryDTO.EnCategoryName
                });
            }

            // Save changes
            await _repo.UpdateCategoryAsync(category);
        }

        public async Task<bool> DeleteCategoryById(int id)
        {
            var exists = await _repo.CategoryExistsAsync(id);
            if (!exists)
                return false;

            return await _repo.DeleteCategoryAsync(id);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _repo.CategoryExistsAsync(id);
        }
    }
}