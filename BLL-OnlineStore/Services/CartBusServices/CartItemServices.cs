using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.Interfaces.CartBusServices;
using DAL_OnlineStore.Repositories.Interfaces.CartItemRepository;
using DAL_OnlineStore.Entities.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.CartItemBusServices
{
    public class CartItemServices : ICartItemServices
    {
        private readonly ICartItemRepo _repo;
        private readonly IMapper _mapper;

        public CartItemServices(ICartItemRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CartItemDTO>?> GetAllCartItems()
        {
            var CartItems = await _repo.getAllCartItems();
            if (CartItems == null)
                return null;

            return _mapper.Map<List<CartItemDTO>>(CartItems);

        }

        public async Task<CartItemDTO?> GetCartItemById(int ID)
        {
            var CartItem = await _repo.getCartItemById(ID);
            if (CartItem == null) return null;

            return _mapper?.Map<CartItemDTO>(CartItem);
        }

        public async Task<CartItemDTO?> AddNewCartItem(CartItemDTO DTO)
        {
            var CartItem = _mapper.Map<CartItem>(DTO);

            var NewCartItem = await _repo.addNewCartItem(CartItem);
            if (NewCartItem != null)
            {
                return _mapper.Map<CartItemDTO>(NewCartItem);
            }
            return null;
        }
        public async Task<bool> UpdateCartItemById(CartItemDTO DTO)
        {
            if (DTO == null)
                return false;

            var CartItem = _mapper.Map<CartItem>(DTO);
            return await _repo.updateCartItemById(CartItem);
        }
        public async Task<bool> DeleteCartItemById(int id)
        {
            return await _repo.deleteCartItemById(id);
        }

        public async Task<int> CountCartItems()
        {
            return await _repo.countCartItems();
        }
    }
}
