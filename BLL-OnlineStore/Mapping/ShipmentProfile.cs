using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using DAL_OnlineStore.Entities.Models.ShipmentModels;

namespace BLL_OnlineStore.Mapping
{
    //Shipment
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            CreateMap<ShipmentDTO, Shipment>();
            CreateMap<Shipment, ShipmentDTO>();
        }
    }
}
