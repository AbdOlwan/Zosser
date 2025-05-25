using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.Interfaces.CartBusServices;
using DAL_OnlineStore.Repositories.Interfaces.CartRepository;
using DAL_OnlineStore.Entities.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.CartBusServices
{
    public class CartServices : ICartServices
    {
        private readonly ICartRepo _repo;
        private readonly IMapper _mapper;

        public CartServices(ICartRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CartDTO>?> GetAllCarts()
        {
            var Carts = await _repo.getAllCarts();
            if (Carts == null)
                return null;

            return _mapper.Map<List<CartDTO>>(Carts);

        }

        public async Task<CartDTO?> GetCartById(int ID)
        {
            var Cart = await _repo.getCartById(ID);
            if (Cart == null) return null;

            return _mapper?.Map<CartDTO>(Cart);
        }

        public async Task<CartDTO?> AddNewCart(CartDTO DTO)
        {
            var Cart = _mapper.Map<Cart>(DTO);

            var NewCart = await _repo.addNewCart(Cart);
            if (NewCart != null)
            {
                return _mapper.Map<CartDTO>(NewCart);
            }
            return null;
        }
        public async Task<bool> UpdateCartById(CartDTO DTO)
        {
            if (DTO == null)
                return false;

            var Cart = _mapper.Map<Cart>(DTO);
            return await _repo.updateCartById(Cart);
        }
        public async Task<bool> DeleteCartById(int id)
        {
            return await _repo.deleteCartById(id);
        }

        //public async Task<int> CountCarts()
        //{
        //    return await _repo.countCarts();
        //}
    }
}
