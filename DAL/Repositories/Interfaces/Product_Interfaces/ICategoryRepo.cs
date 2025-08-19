using DAL.Entities.Models.ProductModels;

namespace DAL.Repositories.Interfaces.Product_Interfaces
{
    public interface ICategoryRepo
    {
        Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken = default);
        Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
        Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
        Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    }
}
