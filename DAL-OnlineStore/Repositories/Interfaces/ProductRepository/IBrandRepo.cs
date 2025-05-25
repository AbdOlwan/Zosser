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
        Task<List<Brand>?> getAllBrands();
        Task<Brand> addNewBrand(Brand Brand);

        //Task<int> countBrands();

        Task<Brand?> getBrandById(int id);

        Task<bool> deleteBrandById(int id);

        Task<bool> updateBrandById(Brand Brand);
    }
}
