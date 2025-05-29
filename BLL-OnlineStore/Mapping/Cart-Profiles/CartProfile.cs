using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using DAL_OnlineStore.Entities.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Mapping
{
    public class CartProfile:Profile
    {
        public CartProfile()
        {
            CreateMap<CartDTO, Cart>();
            CreateMap<Cart, CartDTO>();
        }
    }
}
