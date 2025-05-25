using DAL_OnlineStore.Entities.Models;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ShipmentRepository
{
    public interface ICarrierRepo
    {
        Task<List<Carrier>?> getAllCarriers();
        Task<Carrier> addNewCarrier(Carrier carrier);

        //Task<int> countCarriers();

        Task<Carrier?> getCarrierById(int id);

        Task<bool> deleteCarrierById(int id);

        Task<bool> updateCarrierById(Carrier carrier);
    }
}
