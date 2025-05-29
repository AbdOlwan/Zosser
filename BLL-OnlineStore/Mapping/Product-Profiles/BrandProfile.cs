using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Mapping
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            // من Brand إلى BrandDTO
            CreateMap<Brand, BrandDTO>()
                .ForMember(dest => dest.Brand_ID, opt => opt.MapFrom(src => src.Brand_ID))
                .ForMember(dest => dest.Brand_Name, opt => opt.MapFrom((src, _, __, context) =>
                {
                    var culture = context.Items.TryGetValue("Culture", out var cultureValue)
                        ? cultureValue?.ToString().ToLower()
                        : "ar";

                    var translation = src.BrandTranslations?.FirstOrDefault(t =>
                        (t.Culture ?? "").ToLower() == culture);

                    return translation?.Brand_Name ?? string.Empty;
                }))
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.BrandTranslations ?? new List<BrandTranslation>()));

            // من BrandTranslation إلى BrandTranslationDTO
            CreateMap<BrandTranslation, BrandTranslationDTO>()
                .ForMember(dest => dest.Brand_ID, opt => opt.MapFrom(src => src.Brand_ID))
                .ForMember(dest => dest.Brand_Name, opt => opt.MapFrom(src => src.Brand_Name))
                .ForMember(dest => dest.Culture, opt => opt.MapFrom(src => src.Culture));

            // من CreateBrandDTO إلى Brand
            CreateMap<CreateBrandDTO, Brand>()
                .ForMember(dest => dest.BrandTranslations, opt => opt.MapFrom(src => new List<BrandTranslation>
                {
                new BrandTranslation { Culture = "ar", Brand_Name = src.ArBrandName },
                new BrandTranslation { Culture = "en", Brand_Name = src.EnBrandName }
                }));

            // من UpdateBrandDTO إلى Brand
            CreateMap<UpdateBrandDTO, Brand>()
                .ForMember(dest => dest.Brand_ID, opt => opt.MapFrom(src => src.Brand_ID))
                .ForMember(dest => dest.BrandTranslations, opt => opt.Ignore()); // عادةً يتم تحديث الترجمات بشكل منفصل
        }
    }

}
