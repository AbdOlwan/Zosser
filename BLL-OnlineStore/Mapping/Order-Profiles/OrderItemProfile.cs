using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using DAL_OnlineStore.Entities.Models.OrderModels;

namespace BLL_OnlineStore.Mapping
{
    //OrderItemDTO

    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItemDTO, OrderItem>();
            CreateMap<OrderItem, OrderItemDTO>();
        }
    }
}
