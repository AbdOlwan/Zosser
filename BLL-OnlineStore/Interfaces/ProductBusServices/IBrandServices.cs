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
        Task<IEnumerable<BrandDTO>?> GetAllBrands(string culture = "ar");


        //Task<int> countBrands();
        Task<BrandDTO?> CreateBrandAsync(CreateBrandDTO dto);
        Task<BrandDTO?> GetBrandByIdAsync(int BrandId, string culture = "ar");

        Task<bool> DeleteBrandById(int id);

        Task  UpdateBrandAsync(UpdateBrandDTO updateBrandDTO);

        Task<bool> BrandExistsAsync(int id);
    }
}
//Task<BrandDTO?> AddNewBrand(BrandDTO Brand);