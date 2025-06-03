using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductType = DAL_OnlineStore.Entities.Models.ProductModels.ProductType;

namespace BLL_OnlineStore.Services.ProductBusServices
{
    public class TypeServices : ITypeServices
    {
        private readonly ITypeRepo _repo;
        private readonly IMapper _mapper;

        public TypeServices(ITypeRepo repo, IMapper maper)
        {
            _repo = repo;
            _mapper = maper;
        }

        public async Task<List<TypeDTO>?> GetAllTypes()
        {
            var Types = await _repo.getAllTypes();
            if (Types == null)
                return null;

            return _mapper.Map<List<TypeDTO>>(Types);
        }

        public async Task<TypeDTO?> GetTypeById(int TypeId)
        {
            var Type = await _repo.getTypeById(TypeId);
            if (Type == null) return null;
            //var mapper = configMapper

            return _mapper.Map<TypeDTO>(Type);
        }

        public async Task<TypeDTO?> AddNewType(TypeDTO DTO)
        {
            var _Type = _mapper.Map<ProductType>(DTO);

            var NewType = await _repo.addNewType(_Type);
            if (NewType != null)
            {
                return _mapper.Map<TypeDTO?>(NewType);
            }
            return null;
        }
        public async Task<bool> UpdateTypeById(TypeDTO DTO)
        {
            if (DTO == null)
                return false;

            var Type = _mapper.Map<ProductType>(DTO);
            return await _repo.updateTypeById(Type);
        }
        public async Task<bool> DeleteTypeById(int id)
        {
            return await _repo.deleteTypeById(id);
        }

        public async Task<bool> TypeExistsAsync(int id)
        {
           return await _repo.TypeExistsAsync(id);
        }

    }
}
