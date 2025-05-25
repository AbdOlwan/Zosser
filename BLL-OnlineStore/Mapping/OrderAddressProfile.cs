using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Mapping
{
    public class OrderAddressProfile:Profile
    {
        public OrderAddressProfile()
        {
            CreateMap<OrderAddressDTO, OrderAddress>();
            CreateMap<OrderAddress, OrderAddressDTO>();
        }
    }
}
