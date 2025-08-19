using DAL.Entities.Models.ProductModels;

namespace DAL.Repositories.Interfaces.Product_Interfaces
{
    public interface IProductImageRepo
    {
        Task<ProductImage> AddImageAsync(ProductImage image, CancellationToken cancellationToken = default);
        Task UpdateImageAsync(ProductImage image, CancellationToken cancellationToken = default);
        Task DeleteImageAsync(int id, CancellationToken cancellationToken = default);
        Task<ProductImage?> GetImageByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(
            int productId,
            CancellationToken cancellationToken = default);
    }
}
