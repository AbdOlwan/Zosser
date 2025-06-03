
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using DAL_OnlineStore.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetAllProductsAsync(string culture = "ar", int page = 1, int limit = 10);
        Task<ProductDTO?> GetProductByIdAsync(int id, string culture = "ar");
        Task<ProductDTO?> GetProductBySlugAsync(string slug, string culture = "ar");
        Task<PagedResult<ProductDTO>> GetProductsByCategoryIdAsync(int categoryId, string culture = "ar", int page = 1, int limit = 10);
        Task<PagedResult<ProductDTO>> GetProductsByBrandIdAsync(int brandId, string culture = "ar", int page = 1, int limit = 10);
        Task<PagedResult<ProductDTO>> GetProductsByTypeIdAsync(int typeId, string culture = "ar", int page = 1, int limit = 10);
        Task<ProductWithTranslationsDTO?> GetProductWithTranslationsAsync(int id);
        Task<ProductDTO?> CreateProductAsync(CreateProductDTO createProductDTO);
        Task UpdateProductAsync(UpdateProductDTO updateProductDTO);
        Task  DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
        Task<bool> ProductExistsBySlugAsync(string slug);
    }
}




















//using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BLL_OnlineStore.Interfaces.ProductBusServices
//{
//    public interface IProductServices
//    {
//        Task<List<ProductDTO>?> GetAllProducts();
//        Task<ProductDTO?> AddNewProduct(ProductCreateUpdateDto dto);

//        Task<int> CountProducts();

//        Task<ProductDTO?> GetProductById(int id);

//        Task<bool> DeleteProductById(int id);

//        Task<ProductDTO?> UpdateProductById(int id, ProductCreateUpdateDto dto);
//    }
//}
