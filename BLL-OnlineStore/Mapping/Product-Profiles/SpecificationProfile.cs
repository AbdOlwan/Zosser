using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Mapping
{
    public class SpecificationProfile : Profile
    {
        public SpecificationProfile()
        {
            CreateMap<SpecificationDTO, Specification>();

            CreateMap<Specification, SpecificationDTO>()
                .ForMember(d => d.Specification_Name,
                           opt => opt.MapFrom(src => src.specificationTranslations.FirstOrDefault()!.Specification_Name));
        }
    }
}
