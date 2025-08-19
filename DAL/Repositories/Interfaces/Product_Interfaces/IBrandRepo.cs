using DAL.Entities.Models.ProductModels;

namespace DAL.Repositories.Interfaces.Product_Interfaces
{
    public interface IBrandRepo
    {
        Task<Brand> AddBrandAsync(Brand brand, CancellationToken cancellationToken = default);
        Task UpdateBrandAsync(Brand brand, CancellationToken cancellationToken = default);
        Task DeleteBrandAsync(int id, CancellationToken cancellationToken = default);
        Task<Brand?> GetBrandByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken = default);
    }
}
