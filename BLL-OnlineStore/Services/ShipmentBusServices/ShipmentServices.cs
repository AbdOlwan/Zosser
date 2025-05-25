using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using BLL_OnlineStore.Interfaces.ShipmentBusServices;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces;
using DAL_OnlineStore.Repositories.Interfaces.ShipmentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.ShipmentBusServices
{
    public class ShipmentService : IShipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShipmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShipmentDTO>> GetAllShipmentsAsync()
        {
            var shipments = await _unitOfWork.Shipments.GetAllShipmentsAsync();
            return _mapper.Map<IEnumerable<ShipmentDTO>>(shipments);
        }

        public async Task<ShipmentDTO?> GetShipmentByIdAsync(int id)
        {
            var shipment = await _unitOfWork.Shipments.GetShipmentByIdAsync(id);
            if (shipment == null) return null;
            return _mapper.Map<ShipmentDTO>(shipment);
        }

        public async Task<ShipmentDTO> CreateShipmentAsync(ShipmentDTO createShipmentDTO)
        {
            var shipment = _mapper.Map<Shipment>(createShipmentDTO);
            await _unitOfWork.Shipments.AddShipmentAsync(shipment);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ShipmentDTO>(shipment);
        }

        public async Task UpdateShipmentAsync(ShipmentDTO updateShipmentDTO)
        {
            var shipment = _mapper.Map<Shipment>(updateShipmentDTO);
            await _unitOfWork.Shipments.UpdateShipmentAsync(shipment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteShipmentAsync(int id)
        {
            await _unitOfWork.Shipments.DeleteShipmentAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ShipmentExistsAsync(int id)
        {
            return await _unitOfWork.Shipments.ShipmentExistsAsync(id);
        }
    }
}
