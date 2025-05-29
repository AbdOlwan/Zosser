using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using DAL_OnlineStore.Entities.Models.ShipmentModels;

namespace BLL_OnlineStore.Mapping
{
    public class CarrierProfile : Profile
    {
        public CarrierProfile()
        {
            CreateMap<CarrierDTO, Carrier>();
            CreateMap<Carrier, CarrierDTO>();
        }
    }
}
