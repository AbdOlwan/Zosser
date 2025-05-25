using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductType = DAL_OnlineStore.Entities.Models.ProductModels.ProductType;

namespace BLL_OnlineStore.Mapping
{
    public class TypeProfile:Profile
    {
        public TypeProfile()
        {
            CreateMap<TypeDTO, ProductType>();
            CreateMap<ProductType, TypeDTO>();
        }
    }
}
