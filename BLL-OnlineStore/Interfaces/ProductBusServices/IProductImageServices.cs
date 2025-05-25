using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ProductBusServices
{
    public interface IProductImageServices
    {
        Task<List<ProductImageDTO>?> GetAllProductImages();
        Task<ProductImageDTO?> AddNewProductImage(ProductImageDTO productImage);

        Task<int> CountProductImages(int ProductID);

        Task<ProductImageDTO?> GetProductImageById(int id);

        Task<bool> DeleteProductImageById(int id);

        Task<bool> UpdateProductImageById(ProductImageDTO productImage);
    }
}
