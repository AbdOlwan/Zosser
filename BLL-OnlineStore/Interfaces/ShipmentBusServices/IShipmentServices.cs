using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ShipmentBusServices
{
    public interface IShipmentService
    {
        Task<IEnumerable<ShipmentDTO>> GetAllShipmentsAsync();
        Task<ShipmentDTO?> GetShipmentByIdAsync(int id);
        Task<ShipmentDTO> CreateShipmentAsync(ShipmentDTO createShipmentDTO);
        Task UpdateShipmentAsync(ShipmentDTO updateShipmentDTO);
        Task DeleteShipmentAsync(int id);
        Task<bool> ShipmentExistsAsync(int id);
    }
}
