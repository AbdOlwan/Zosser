
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


            CreateMap<Product, ProductDTO>()
                   // الترجمة حسب الثقافة
                   .ForMember(dest => dest.ProductName, opt => opt.MapFrom((src, _, __, context) =>
                       src.productTranslations
                           .FirstOrDefault(pt => pt.Culture == (context.Items.TryGetValue("Culture", out var culture) ? culture?.ToString() : "ar"))?.ProductName ?? string.Empty))
                   .ForMember(dest => dest.Description, opt => opt.MapFrom((src, _, __, context) =>
                       src.productTranslations
                           .FirstOrDefault(pt => pt.Culture == (context.Items.TryGetValue("Culture", out var culture) ? culture?.ToString() : "ar"))?.Description ?? string.Empty))
                   .ForMember(dest => dest.MarketingSlogan, opt => opt.MapFrom((src, _, __, context) =>
                       src.productTranslations
                           .FirstOrDefault(pt => pt.Culture == (context.Items.TryGetValue("Culture", out var culture) ? culture?.ToString() : "ar"))?.MarketingSlogan ?? string.Empty))

                   // ترجمة البراند
                   .ForMember(dest => dest.Brand_Name, opt => opt.MapFrom((src, _, __, context) =>
                       src.brand!.BrandTranslations.FirstOrDefault(bt =>
                           bt.Culture == (context.Items.TryGetValue("Culture", out var culture) ? culture?.ToString() : "ar"))?.Brand_Name ?? string.Empty))
                   
                   // ترجمة الكاتيجوري
                   .ForMember(dest => dest.Category_Name, opt => opt.MapFrom((src, _, __, context) =>
                       src.category?.Translations.FirstOrDefault(ct =>
                          ct.Culture == (context.Items.TryGetValue("Culture", out var culture) ? culture?.ToString() : "ar"))?.Category_Name ?? string.Empty))

                   .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.type != null ? src.type.TypeName : string.Empty))

                   // الصور
                   .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images != null ? src.Images.Select(i => i.ImageURL).ToList() : new List<string>()));





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
