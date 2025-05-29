using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using DAL_OnlineStore.Entities.Models.OrderModels;

namespace BLL_OnlineStore.Mapping
{
    //OrderDTO

    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderDTO>();
        }
    }
}
