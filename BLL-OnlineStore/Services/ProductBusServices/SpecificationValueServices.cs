using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
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
    public class SpecificationValueServices : ISpecificationValueServices
    {
        private readonly ISpecificationValueRepo _repo;
        private readonly IMapper _mapper;

        public SpecificationValueServices(ISpecificationValueRepo repo, IMapper maper)
        {
            _repo = repo;
            _mapper = maper;
        }

        public async Task<List<SpecificationValueDTO>?> GetAllSpecificationValues()
        {
            var SpecificationValues = await _repo.getAllSpecificationValues();
            if (SpecificationValues == null)
                return null;

            return _mapper.Map<List<SpecificationValueDTO>>(SpecificationValues);
        }

        public async Task<SpecificationValueDTO?> GetSpecificationValueById(int SpecificationValueId)
        {
            var SpecificationValue = await _repo.getSpecificationValueById(SpecificationValueId);
            if (SpecificationValue == null) return null;
            //var mapper = configMapper

            return _mapper.Map<SpecificationValueDTO>(SpecificationValue);
        }

        public async Task<SpecificationValueDTO?> AddNewSpecificationValue(SpecificationValueDTO DTO)
        {
            var _SpecificationValue = _mapper.Map<SpecificationValue>(DTO);

            var NewSpecificationValue = await _repo.addNewSpecificationValue(_SpecificationValue);
            if (NewSpecificationValue != null)
            {
                return _mapper.Map<SpecificationValueDTO?>(NewSpecificationValue);
            }
            return null;
        }
        public async Task<bool> UpdateSpecificationValueById(SpecificationValueDTO DTO)
        {
            if (DTO == null)
                return false;

            var SpecificationValue = _mapper.Map<SpecificationValue>(DTO);
            return await _repo.updateSpecificationValueById(SpecificationValue);
        }
        public async Task<bool> DeleteSpecificationValueById(int id)
        {
            return await _repo.deleteSpecificationValueById(id);
        }
    }
}
