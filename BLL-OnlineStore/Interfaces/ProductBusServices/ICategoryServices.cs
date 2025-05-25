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
        Task<List<CategoryDTO>?> GetAllCategorys();
        Task<CategoryDTO?> AddNewCategory(CategoryDTO Category);

        //Task<int> countCategorys();

        Task<CategoryDTO?> GetCategoryById(int id);

        Task<bool> DeleteCategoryById(int id);

        Task<bool> UpdateCategoryById(CategoryDTO Category);
    }
}
