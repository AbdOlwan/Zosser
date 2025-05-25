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
            CreateMap<BrandDTO, Brand>();

            CreateMap<Brand, BrandDTO>()
                .ForMember(d => d.Brand_Name,
                           opt => opt.MapFrom(src => src.BrandTranslations
                                     .FirstOrDefault()!.Brand_Name)) ;
        }
    }
}
