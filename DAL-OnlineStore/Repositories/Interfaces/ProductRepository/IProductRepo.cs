
using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces
{
    public interface IProductRepo
    {
        Task<PagedResult<Product>> GetAllProductsAsync(string culture, int page, int limit);
        Task<Product?> GetProductByIdAsync(int id, string? culture);
        Task<Product?> GetProductBySlugAsync(string slug, string culture);
        Task<PagedResult<Product>> GetProductsByCategoryIdAsync(int categoryId, string culture, int page, int limit);
        Task<PagedResult<Product>> GetProductsByBrandIdAsync(int brandId, string culture, int page, int limit);
        Task<PagedResult<Product>> GetProductsByTypeIdAsync(int typeId, string culture, int page, int limit);
        Task<IEnumerable<Product>> FindProductsAsync(Expression<Func<Product, bool>> predicate, string culture);
        Task<Product> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task  DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
        Task<bool> ProductExistsBySlugAsync(string slug);
    }
}





//using DAL_OnlineStore.Entities.Models.PaymentModels;
//using DAL_OnlineStore.Entities.Models.ProductModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
//{
//    public interface IProductRepo
//    {
//        Task<List<Product>?> getAllProducts();
//        Task<Product> addNewProduct(Product product);

//        Task<int> countProducts();

//        Task<Product?> getProductById(int id);

//        Task<bool> deleteProductById(int id);

//        Task<Product?> updateProduct(Product product);
//    }
//}
