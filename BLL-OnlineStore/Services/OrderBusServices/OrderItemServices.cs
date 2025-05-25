using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Order_F;
using BLL_OnlineStore.Interfaces.OrderBusServices;
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Repositories.Interfaces;
using DAL_OnlineStore.Repositories.Interfaces.OrderRepository;

namespace BLL_OnlineStore.Services.OrderBusServices
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderItemDTO>> GetItemsByOrderIdAsync(int orderId)
        {
            var items = await _unitOfWork.OrderItems.GetItemsByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemDTO>>(items);
        }

        public async Task<OrderItemDTO?> GetOrderItemByIdAsync(int id)
        {
            var item = await _unitOfWork.OrderItems.GetOrderItemByIdAsync(id);
            if (item == null) return null;
            return _mapper.Map<OrderItemDTO>(item);
        }

        public async Task<OrderItemDTO> CreateOrderItemAsync(OrderItemDTO createOrderItemDTO)
        {
            var item = _mapper.Map<OrderItem>(createOrderItemDTO);
            await _unitOfWork.OrderItems.AddOrderItemAsync(item);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<OrderItemDTO>(item);
        }

        public async Task UpdateOrderItemAsync(OrderItemDTO updateOrderItemDTO)
        {
            var item = _mapper.Map<OrderItem>(updateOrderItemDTO);
            await _unitOfWork.OrderItems.UpdateOrderItemAsync(item);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            await _unitOfWork.OrderItems.DeleteOrderItemAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> OrderItemExistsAsync(int id)
        {
            return await _unitOfWork.OrderItems.OrderItemExistsAsync(id);
        }
    }

}
