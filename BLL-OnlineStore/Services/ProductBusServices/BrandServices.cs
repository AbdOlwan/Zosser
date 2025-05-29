using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using BLL_OnlineStore.Interfaces;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Implementations.ProductRepository;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public async Task<IEnumerable<BrandDTO>?> GetAllBrands(string culture = "ar")
        {
            var Brands = await _repo.GetAllBrandsAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }
        public async Task<BrandDTO?> GetBrandByIdAsync(int BrandId, string culture = "ar")
        {
            var Brand = await _repo.getBrandById(BrandId, includeTranslations: true);
            if (Brand == null)
                return null;

            // تأكد إن الترجمات موجودة
            if (Brand.BrandTranslations == null || !Brand.BrandTranslations.Any())
            {
                // لو مفيش ترجمات، ارجع CategoryDTO فارغ بس بالبيانات الأساسية
                return new BrandDTO
                {
                    Brand_ID = Brand.Brand_ID,
                    Slug = Brand.Slug,
                    Brand_Name = string.Empty,
                    Translations = []
                };
            }
            // Map مع Context للـ Culture
            var result = _mapper.Map<BrandDTO>(Brand, opt => opt.Items["Culture"] = culture.ToLower());

            // تأكد إن الـ Translations اتعملها Map صح
            if (result.Translations == null || !result.Translations.Any())
            {
                result.Translations = Brand.BrandTranslations.Select(t => new BrandTranslationDTO
                {
                    Id = t.Id,
                    Brand_ID = t.Brand_ID,
                    Culture = t.Culture,
                    Brand_Name = t.Brand_Name
                }).ToList();
            }

            return result;
        }

        public async Task<BrandDTO?> CreateBrandAsync(CreateBrandDTO dto)
        {
            var brand = _mapper.Map<Brand>(dto);

            await _repo.addNewBrand(brand);

            return await GetBrandByIdAsync(brand.Brand_ID);
        }

       
        public async Task  UpdateBrandAsync(UpdateBrandDTO updateBrandDTO)
        {
            // Get existing brand with all translations
            var brand = await _repo.getBrandById(updateBrandDTO.Brand_ID, includeTranslations: true);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {updateBrandDTO.Brand_ID} not found");

            // Update base brand properties
            _mapper.Map(updateBrandDTO, brand);

            // Update Arabic translation
            var arTranslation = brand.BrandTranslations.FirstOrDefault(bt => bt.Culture == "ar");
            if (arTranslation != null)
            {
                arTranslation.Brand_Name = updateBrandDTO.ArBrandName;
            }
            else
            {
                brand.BrandTranslations.Add(new BrandTranslation
                {
                    Culture = "ar",
                    Brand_Name = updateBrandDTO.ArBrandName
                });
            }

            // Update English translation
            var enTranslation = brand.BrandTranslations.FirstOrDefault(bt => bt.Culture == "en");
            if (enTranslation != null)
            {
                enTranslation.Brand_Name = updateBrandDTO.EnBrandName;
            }
            else
            {
                brand.BrandTranslations.Add(new BrandTranslation
                {
                    Culture = "en",
                    Brand_Name = updateBrandDTO.EnBrandName
                });
            }

            // Save changes
            await _repo.UpdateBrand(brand);
        }


        public async Task<bool> DeleteBrandById(int id)
        {
            return await _repo.deleteBrandById(id);
        }


        public async Task<bool> BrandExistsAsync(int id)
        {
            return await _repo.BrandExistsAsync(id);
        }

    }

     //public async Task<bool> UpdateBrandById(BrandDTO DTO)
     //   {
     //       if (DTO == null)
     //           return false;

     //       var Brand = _mapper.Map<Brand>(DTO);
     //       return await _repo.updateBrandById(Brand);
     //   }
    }
//public async Task<BrandDTO?> AddNewBrand(BrandDTO DTO)
//{
//    var _Brand = _mapper.Map<Brand>(DTO);

//    var NewBrand = await _repo.addNewBrand(_Brand);
//    if (NewBrand != null)
//    {
//        return _mapper.Map<BrandDTO?>(NewBrand);
//    }
//    return null;
//}
