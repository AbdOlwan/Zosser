using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
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
    public class BrandServices : IBrandServices
    {
        private readonly IBrandRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICultureService _culture;

        public BrandServices(
            IBrandRepo repo,
            IMapper maper,
            ICultureService culture)
        {
            _repo = repo;
            _mapper = maper;
            _culture = culture;
        }

        public async Task<List<BrandDTO>?> GetAllBrands()
        {
            var Brands = await _repo.getAllBrands();
            if (Brands == null)
                return null;

            return _mapper.Map<List<BrandDTO>>(Brands);
        }

        public async Task<BrandDTO?> GetBrandById(int BrandId)
        {
            var Brand = await _repo.getBrandById(BrandId);
            if (Brand == null) return null;
            //var mapper = configMapper

            return _mapper.Map<BrandDTO>(Brand);
        }
        public async Task<BrandDTO?> AddNewBrand(BrandDTO DTO)
        {
            var _Brand = _mapper.Map<Brand>(DTO);

            var NewBrand = await _repo.addNewBrand(_Brand);
            if (NewBrand != null)
            {
                return _mapper.Map<BrandDTO?>(NewBrand);
            }
            return null;
        }
        public async Task<bool> UpdateBrandById(BrandDTO DTO)
        {
            if (DTO == null)
                return false;

            var Brand = _mapper.Map<Brand>(DTO);
            return await _repo.updateBrandById(Brand);
        }
        public async Task<bool> DeleteBrandById(int id)
        {
            return await _repo.deleteBrandById(id);
        }
    }
}
