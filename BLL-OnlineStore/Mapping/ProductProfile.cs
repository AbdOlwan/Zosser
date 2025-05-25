
using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Linq;

namespace BLL_OnlineStore.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Map from Product to ProductDTO (considering translations)
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom((src, _, __, context) =>
                    src.productTranslations
                        .FirstOrDefault(pt => pt.Culture == (context.Items.TryGetValue("Culture", out var culture) ? culture?.ToString() : "ar"))?.ProductName ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, _, __, context) =>
                    src.productTranslations
                        .FirstOrDefault(pt => pt.Culture == (context.Items.TryGetValue("Culture", out var culture) ? culture?.ToString() : "ar"))?.Description ?? string.Empty))
                .ForMember(dest => dest.MarketingSlogan, opt => opt.MapFrom((src, _, __, context) =>
                    src.productTranslations
                        .FirstOrDefault(pt => pt.Culture == (context.Items.TryGetValue("Culture", out var culture) ? culture?.ToString() : "ar"))?.MarketingSlogan ?? string.Empty));

            // Map from ProductDTO to Product
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.productTranslations, opt => opt.Ignore());

            // Map from ProductTranslation to ProductDTO (single translation)
            CreateMap<ProductTranslation, ProductDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.MarketingSlogan, opt => opt.MapFrom(src => src.MarketingSlogan));

            // Maps for create/update scenarios
            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.productTranslations, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.productTranslations, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Map for product with translations
            CreateMap<ProductWithTranslationsDTO, Product>();
            CreateMap<Product, ProductWithTranslationsDTO>();

            // Map for translations
            CreateMap<ProductTranslationDTO, ProductTranslation>();
            CreateMap<ProductTranslation, ProductTranslationDTO>();
        }
    }
}







//using AutoMapper;
//using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
//using DAL_OnlineStore.Entities.Models.ProductModels;

//namespace BLL_OnlineStore.Mapping
//{
//    public class ProductMappingProfile : Profile
//    {
//        public ProductMappingProfile()
//        {
//            // Product to ProductDTO mapping with language resolver
//            CreateMap<Product, ProductDTO>()
//                .ForMember(dest => dest.ProductName, opt => opt.MapFrom<LanguageResolver, string>(src => nameof(ProductTranslation.ProductName)))
//                .ForMember(dest => dest.Description, opt => opt.MapFrom<LanguageResolver, string>(src => nameof(ProductTranslation.Description)))
//                .ForMember(dest => dest.MarketingSlogan, opt => opt.MapFrom<LanguageResolver, string>(src => nameof(ProductTranslation.MarketingSlogan)));

//            // ProductDTO to Product mapping (for create/update operations)
//            CreateMap<ProductDTO, Product>()
//                .ForMember(dest => dest.productTranslations, opt => opt.Ignore())
//                .AfterMap((src, dest, ctx) => {
//                    // Get the language from resolution context
//                    var language = ctx.Items.ContainsKey("Language") ? ctx.Items["Language"].ToString() : "ar";

//                    // Initialize the collection if it's null
//                    if (dest.productTranslations == null)
//                    {
//                        dest.productTranslations = new List<ProductTranslation>();
//                    }

//                    // Find existing translation for current language or create new one
//                    var translation = dest.productTranslations
//                        .FirstOrDefault(pt => pt.Culture == language);

//                    if (translation == null)
//                    {
//                        translation = new ProductTranslation
//                        {
//                            ProductId = dest.ProductId,
//                            Culture = language!
//                        };
//                        dest.productTranslations.Add(translation);
//                    }

//                    // Update the translation properties
//                    translation.ProductName = src.ProductName;
//                    translation.Description = src.Description;
//                    translation.MarketingSlogan = src.MarketingSlogan;
//                });
//        }
//    }

//    // Custom resolver to handle language-specific mappings
//    public class LanguageResolver : IMemberValueResolver<Product, ProductDTO, string, string>
//    {
//        public string Resolve(Product source, ProductDTO destination, string sourceMember, string destMember, ResolutionContext context)
//        {
//            // Get language from resolution context - default to "en" if not specified
//            var language = context.Items.ContainsKey("Language") ? context.Items["Language"].ToString() : "ar";

//            // Find translation for the requested language
//            var translation = source.productTranslations?.FirstOrDefault(pt => pt.Culture == language);

//            // If no translation in requested language, fallback to default language ("en" or the first available)
//            if (translation == null)
//            {
//                // Try English as fallback
//                translation = source.productTranslations?.FirstOrDefault(pt => pt.Culture == "ar");

//                // If still no translation, take the first available
//                if (translation == null)
//                {
//                    translation = source.productTranslations?.FirstOrDefault();
//                }
//            }

//            // Get the property value using reflection based on sourceMember name
//            if (translation != null)
//            {
//                var property = typeof(ProductTranslation).GetProperty(sourceMember);
//                if (property != null)
//                {
//                    return property.GetValue(translation)?.ToString() ?? string.Empty;
//                }
//            }

//            return string.Empty;
//        }
//    }
//}