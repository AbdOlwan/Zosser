using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
{
    public interface ICategoryRepo
    {
        // Basic CRUD Operations
        Task<IEnumerable<Category>> GetAllCategoriesAsync(
            Expression<Func<Category, bool>>? filter = null,
            Func<IQueryable<Category>, IQueryable<Category>>? include = null,
            bool asNoTracking = true);

        Task<IEnumerable<Category>> GetAllCategoriesWithTranslationsAsync(string? culture = null);

        Task<Category?> GetCategoryByIdAsync(int id, bool includeTranslations = true, bool asNoTracking = false);

        Task<Category?> GetCategoryBySlugAsync(string slug, bool includeTranslations = true, bool asNoTracking = false);

        Task<Category> AddCategoryAsync(Category category);

        Task<bool> UpdateCategoryAsync(Category category);

        Task<bool> DeleteCategoryAsync(int id);

        // Validation Methods
        Task<bool> CategoryExistsAsync(int id);

        Task<bool> SlugExistsAsync(string slug, int? excludeCategoryId = null);

        // Utility Methods
        Task<int> GetCategoriesCountAsync(Expression<Func<Category, bool>>? filter = null);

        Task<IEnumerable<Category>> GetPaginatedCategoriesAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<Category, bool>>? filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null);
    }
}