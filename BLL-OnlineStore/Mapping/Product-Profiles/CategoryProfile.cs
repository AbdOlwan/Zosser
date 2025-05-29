using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_OnlineStore.Mapping
{
    public static class MappingConstants
    {
        public const string DefaultCulture = "ar";
        public const string EnglishCulture = "en";
    }

    // Resolver for category name by culture
    public class CategoryNameResolver : IValueResolver<Category, CategoryDTO, string>
    {
        public string Resolve(Category source, CategoryDTO destination, string destMember, ResolutionContext context)
        {
            // التأكد من وجود translations
            if (source.Translations == null || !source.Translations.Any())
                return string.Empty;

            // قراءة الـ culture من الـ context
            var culture = context.Items.TryGetValue("Culture", out var cultureValue)
                ? cultureValue?.ToString()?.ToLower()?.Trim()
                : MappingConstants.DefaultCulture;

            // البحث عن الترجمة المطلوبة
            var translation = source.Translations
                .FirstOrDefault(t => !string.IsNullOrEmpty(t.Culture) &&
                                   t.Culture.Trim().Equals(culture, StringComparison.OrdinalIgnoreCase));

            // لو مش موجودة، ابحث عن الـ default culture
            if (translation == null)
            {
                translation = source.Translations
                    .FirstOrDefault(t => !string.IsNullOrEmpty(t.Culture) &&
                                       t.Culture.Trim().Equals(MappingConstants.DefaultCulture, StringComparison.OrdinalIgnoreCase));
            }

            // لو برضو مش موجودة، خد أول ترجمة متاحة
            if (translation == null)
            {
                translation = source.Translations.FirstOrDefault(t => !string.IsNullOrEmpty(t.Category_Name));
            }

            return translation?.Category_Name?.Trim() ?? string.Empty;
        }
    }

    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Entity -> DTO
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => src.Slug))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom<CategoryNameResolver>())
                .ForMember(dest => dest.Translations,
                    opt => opt.MapFrom(src => src.Translations ?? new List<CategoryTranslation>()));

            // Translation entity -> DTO
            CreateMap<CategoryTranslation, CategoryTranslationDTO>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.Culture,
                    opt => opt.MapFrom(src => src.Culture))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category_Name));

            // Create DTO -> Entity
            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.CategoryID,
                    opt => opt.Ignore()) // Auto-generated
                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => src.Slug))
                .ForMember(dest => dest.Translations,
                    opt => opt.MapFrom(src => CreateTranslationsFromCreateDTO(src)))
                .ForMember(dest => dest.Products,
                    opt => opt.Ignore()); // Navigation property

            // Update DTO -> Entity
            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(dest => dest.CategoryID,
                    opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => src.Slug))
                .ForMember(dest => dest.Translations,
                    opt => opt.Ignore()) // Handle separately in service
                .ForMember(dest => dest.Products,
                    opt => opt.Ignore()); // Navigation property
        }

        // Helper method to create translations
        private static List<CategoryTranslation> CreateTranslationsFromCreateDTO(CreateCategoryDTO dto)
        {
            var translations = new List<CategoryTranslation>();

            // Add Arabic translation if provided
            if (!string.IsNullOrWhiteSpace(dto.ArCategoryName))
            {
                translations.Add(new CategoryTranslation
                {
                    Culture = MappingConstants.DefaultCulture,
                    Category_Name = dto.ArCategoryName.Trim()
                });
            }

            // Add English translation if provided
            if (!string.IsNullOrWhiteSpace(dto.EnCategoryName))
            {
                translations.Add(new CategoryTranslation
                {
                    Culture = MappingConstants.EnglishCulture,
                    Category_Name = dto.EnCategoryName.Trim()
                });
            }

            return translations;
        }
    }
}