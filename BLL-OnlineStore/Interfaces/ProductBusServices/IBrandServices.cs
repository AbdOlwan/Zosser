using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ProductBusServices
{
    public interface IBrandServices
    {
        Task<List<BrandDTO>?> GetAllBrands();
        Task<BrandDTO?> AddNewBrand(BrandDTO Brand);

        //Task<int> countBrands();

        Task<BrandDTO?> GetBrandById(int id);

        Task<bool> DeleteBrandById(int id);

        Task<bool> UpdateBrandById(BrandDTO Brand);
    }
}
