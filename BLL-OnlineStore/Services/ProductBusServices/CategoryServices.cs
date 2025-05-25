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
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICultureService _culture;


        public CategoryServices
            (ICategoryRepo repo,
            IMapper maper,
            ICultureService culture
            )
        {
            _repo = repo;
            _mapper = maper;
            _culture = culture;
        }

        public async Task<List<CategoryDTO>?> GetAllCategorys()
        {
            var Categorys = await _repo.getAllCategorys();
            if (Categorys == null)
                return null;

            return _mapper.Map<List<CategoryDTO>>(Categorys);
        }

        public async Task<CategoryDTO?> GetCategoryById(int CategoryId)
        {
            var Category = await _repo.getCategoryById(CategoryId);
            if (Category == null) return null;
            //var mapper = configMapper

            return _mapper.Map<CategoryDTO>(Category);
        }

        public async Task<CategoryDTO?> AddNewCategory(CategoryDTO DTO)
        {
            var _Category = _mapper.Map<Category>(DTO);

            var NewCategory = await _repo.addNewCategory(_Category);
            if (NewCategory != null)
            {
                return _mapper.Map<CategoryDTO?>(NewCategory);
            }
            return null;
        }
        public async Task<bool> UpdateCategoryById(CategoryDTO DTO)
        {
            if (DTO == null)
                return false;

            var Category = _mapper.Map<Category>(DTO);
            return await _repo.updateCategoryById(Category);
        }
        public async Task<bool> DeleteCategoryById(int id)
        {
            return await _repo.deleteCategoryById(id);
        }
    }
}
