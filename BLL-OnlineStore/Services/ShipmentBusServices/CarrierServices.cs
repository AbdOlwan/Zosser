using DAL_OnlineStore.Entities.Models.ShipmentModels;
using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using DAL_OnlineStore.Repositories.Interfaces.ShipmentRepository;
using BLL_OnlineStore.Interfaces.ShipmentBusServices;




namespace BLL_OnlineStore.Services.ShipmentBusServices
{
    public class CarrierServices : ICarrierServices
    {
        private readonly ICarrierRepo _repo;
        private readonly IMapper _mapper;

        public CarrierServices(ICarrierRepo repo, IMapper maper)
        {
            _repo = repo;
            _mapper = maper;
        }

        public async Task<List<CarrierDTO>?> GetAllCarriers()
        {
            var Carriers = await _repo.getAllCarriers();
            if (Carriers == null)
                return null;

            return _mapper.Map<List<CarrierDTO>>(Carriers);





        }

        public async Task<CarrierDTO?> GetCarrierById(int CarrierId)
        {
            var Carrier = await _repo.getCarrierById(CarrierId);
            if (Carrier == null) return null;
            //var mapper = configMapper

            return _mapper.Map<CarrierDTO>(Carrier);
        }

        public async Task<CarrierDTO?> AddNewCarrier(CarrierDTO carrier)
        {
            var Carrier = _mapper.Map<Carrier>(carrier);

            var NewCarrier = await _repo.addNewCarrier(Carrier);
            if (NewCarrier != null)
            {
                return _mapper.Map<CarrierDTO?>(NewCarrier);
            }
            return null;
        }
        public async Task<bool> UpdateCarrierById(CarrierDTO carrier)
        {
            if (carrier == null)
                return false;

            var Carrier = _mapper.Map<Carrier>(carrier);
            return await _repo.updateCarrierById(Carrier);
        }
        public async Task<bool> DeleteCarrierById(int id)
        {
            return await _repo.deleteCarrierById(id);
        }

    }
}
