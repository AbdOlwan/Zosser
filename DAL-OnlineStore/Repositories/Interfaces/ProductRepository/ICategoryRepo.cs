using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
{
    public interface ICategoryRepo
    {
        Task<List<Category>?> getAllCategorys();
        Task<Category> addNewCategory(Category Category);

        //Task<int> countCategorys();

        Task<Category?> getCategoryById(int id);

        Task<bool> deleteCategoryById(int id);

        Task<bool> updateCategoryById(Category Category);
    }
}
