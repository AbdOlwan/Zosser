using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using DAL_OnlineStore.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ProductBusServices
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategorys(string culture = "ar");
        Task<CategoryDTO?> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO);

        //Task<int> countCategorys();

        Task<CategoryDTO?> GetCategoryByIdAsync(int id, string culture = "ar");

        Task<bool> DeleteCategoryById(int id);

        Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO);

        Task<bool> CategoryExistsAsync(int id);
    }
}
