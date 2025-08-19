using DAL.Entities.Models.ProductModels;
using Shared.DTOs.ProductDTOs;

namespace DAL.Repositories.Interfaces.Product_Interfaces
{
    public interface IProductRepo
    {
        Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);
        Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task DeleteProductAsync(int id, CancellationToken cancellationToken = default);
        Task<Product?> GetProductByIdAsync(int id, bool track = false);
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default);
        Task<bool> ProductExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<(IEnumerable<ProductCardDto> products, int totalCount)> GetProductCardsPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
    //Task<(IEnumerable<ProductCardDto> products, int totalCount)> GetProductCardsPagedAsync(
    //    int pageNumber,
    //    int pageSize,
    //    CancellationToken cancellationToken = default);
}

