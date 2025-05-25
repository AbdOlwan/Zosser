using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using DAL_OnlineStore.Entities.Models.ProductModels;

namespace BLL_OnlineStore.Mapping
{
    //ProductImage
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImageDTO, ProductImage>();
            CreateMap<ProductImage, ProductImageDTO>();
        }
    }
}
