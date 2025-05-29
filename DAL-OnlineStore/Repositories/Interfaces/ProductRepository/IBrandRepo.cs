using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
{
    public interface IBrandRepo
    {
        Task<IEnumerable<Brand>?> GetAllBrandsAsync();
        Task<Brand> addNewBrand(Brand Brand);

        //Task<int> countBrands();

        Task<Brand?> getBrandById(int brandId, bool includeTranslations = false);

        Task<bool> deleteBrandById(int id);

        Task UpdateBrand(Brand brand);

        Task<bool> BrandExistsAsync(int id);
    }
}
