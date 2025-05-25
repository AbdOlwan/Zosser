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
    public class SpecificationValueProfile:Profile
    {
        public SpecificationValueProfile()
        {
            CreateMap<SpecificationValueDTO, SpecificationValue>();
            CreateMap<SpecificationValue, SpecificationValueDTO>();
        }
    }
}
