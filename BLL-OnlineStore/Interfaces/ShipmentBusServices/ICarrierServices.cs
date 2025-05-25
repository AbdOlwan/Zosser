using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ShipmentBusServices
{
    public interface ICarrierServices
    {
        Task<List<CarrierDTO>?> GetAllCarriers();
        Task<CarrierDTO?> AddNewCarrier(CarrierDTO carrier);

        //Task<int> countCarriers();

        Task<CarrierDTO?> GetCarrierById(int id);

        Task<bool> DeleteCarrierById(int id);

        Task<bool> UpdateCarrierById(CarrierDTO carrier);
    }
}
