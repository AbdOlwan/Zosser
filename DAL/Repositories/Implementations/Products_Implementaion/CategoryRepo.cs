using DAL.Context;
using DAL.Entities.Models.ProductModels;
using DAL.Repositories.Interfaces.Product_Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Shared.Utils;

namespace DAL.Repositories.Implementations.Products_Implementaion
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepo> _logger;
        private readonly ICurrentUserService _currentUserService;

        public CategoryRepo(
            ApplicationDbContext context,
            ILogger<CategoryRepo> logger,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<Category> AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var userId = _currentUserService.GetCurrentUserId();
                category.CreatedAt = DateTime.UtcNow;
                category.CreatedBy = userId;
                category.LastModifiedAt = DateTime.UtcNow;
                category.LastModifiedBy = userId;

                await _context.Categories.AddAsync(category, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category: {CategoryName}", category.Name);
                throw new ApplicationException("Failed to add category", ex);
            }
        }

        public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                category.LastModifiedAt = DateTime.UtcNow;
                category.LastModifiedBy = _currentUserService.GetCurrentUserId();

                _context.Categories.Update(category);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating category ID: {CategoryId}", category.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category ID: {CategoryId}", category.Id);
                throw new ApplicationException($"Failed to update category {category.Id}", ex);
            }
        }

        public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category not found for deletion: {CategoryId}", id);
                    return;
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category ID: {CategoryId}", id);
                throw new ApplicationException($"Failed to delete category {id}", ex);
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.Categories
                    .AsNoTracking()
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by ID: {CategoryId}", id);
                throw new ApplicationException($"Failed to get category {id}", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.Categories
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all categories");
                throw new ApplicationException("Failed to get categories", ex);
            }
        }
    }
}