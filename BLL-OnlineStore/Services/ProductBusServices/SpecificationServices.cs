using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.ProductBusServices
{
    public class SpecificationServices : ISpecificationServices
    {
        private readonly ISpecificationRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICultureService _culture;
        public SpecificationServices
            (ISpecificationRepo repo,
            IMapper maper,
            ICultureService culture)
        {
            _repo = repo;
            _mapper = maper;
            _culture = culture;
        }

        public async Task<List<SpecificationDTO>?> GetAllSpecifications()
        {
            var Specifications = await _repo.getAllSpecifications();
            if (Specifications == null)
                return null;

            return _mapper.Map<List<SpecificationDTO>>(Specifications);
        }

        public async Task<SpecificationDTO?> GetSpecificationById(int SpecificationId)
        {
            var Specification = await _repo.getSpecificationById(SpecificationId);
            if (Specification == null) return null;
            //var mapper = configMapper

            return _mapper.Map<SpecificationDTO>(Specification);
        }

        public async Task<SpecificationDTO?> AddNewSpecification(SpecificationDTO DTO)
        {
            var _Specification = _mapper.Map<Specification>(DTO);

            var NewSpecification = await _repo.addNewSpecification(_Specification);
            if (NewSpecification != null)
            {
                return _mapper.Map<SpecificationDTO?>(NewSpecification);
            }
            return null;
        }
        public async Task<bool> UpdateSpecificationById(SpecificationDTO DTO)
        {
            if (DTO == null)
                return false;

            var Specification = _mapper.Map<Specification>(DTO);
            return await _repo.updateSpecificationById(Specification);
        }
        public async Task<bool> DeleteSpecificationById(int id)
        {
            return await _repo.deleteSpecificationById(id);
        }
    }
}
