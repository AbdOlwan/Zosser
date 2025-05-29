using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.ProductRepository
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Get all categories with optional filtering and including
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(
            Expression<Func<Category, bool>>? filter = null,
            Func<IQueryable<Category>, IQueryable<Category>>? include = null,
            bool asNoTracking = true)
        {
            try
            {
                var query = _context.Categories.AsQueryable();

                // Apply filtering
                if (filter != null)
                    query = query.Where(filter);

                // Apply includes
                if (include != null)
                    query = include(query);
                else
                    query = query.Include(c => c.Translations); // Default include

                // Apply no tracking if requested
                if (asNoTracking)
                    query = query.AsNoTracking();

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving categories", ex);
            }
        }

        // Get categories with translations only
        public async Task<IEnumerable<Category>> GetAllCategoriesWithTranslationsAsync(string? culture = null)
        {
            try
            {
                var query = _context.Categories
                    .Include(c => c.Translations)
                    .AsNoTracking();

                // Filter by culture if specified
                if (!string.IsNullOrWhiteSpace(culture))
                {
                    query = query.Where(c => c.Translations.Any(t => t.Culture == culture.ToLower()));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving categories for culture: {culture}", ex);
            }
        }

        // Get category by ID
        public async Task<Category?> GetCategoryByIdAsync(int id, bool includeTranslations = true, bool asNoTracking = false)
        {
            try
            {
                if (id <= 0)
                    return null;

                var query = _context.Categories.AsQueryable();

                if (includeTranslations)
                    query = query.Include(c => c.Translations);

                if (asNoTracking)
                    query = query.AsNoTracking();

                return await query.FirstOrDefaultAsync(c => c.CategoryID == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving category with ID: {id}", ex);
            }
        }

        // Get category by slug
        public async Task<Category?> GetCategoryBySlugAsync(string slug, bool includeTranslations = true, bool asNoTracking = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(slug))
                    return null;

                var query = _context.Categories.AsQueryable();

                if (includeTranslations)
                    query = query.Include(c => c.Translations);

                if (asNoTracking)
                    query = query.AsNoTracking();

                return await query.FirstOrDefaultAsync(c => c.Slug == slug.ToLower().Trim());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving category with slug: {slug}", ex);
            }
        }

        // Add new category
        public async Task<Category> AddCategoryAsync(Category category)
        {
            try
            {
                if (category == null)
                    throw new ArgumentNullException(nameof(category));

                // Validate slug uniqueness
                if (!string.IsNullOrWhiteSpace(category.Slug))
                {
                    var existingCategory = await _context.Categories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Slug == category.Slug.ToLower().Trim());

                    if (existingCategory != null)
                        throw new InvalidOperationException($"Category with slug '{category.Slug}' already exists");
                }

                // Normalize slug
                if (!string.IsNullOrWhiteSpace(category.Slug))
                    category.Slug = category.Slug.ToLower().Trim();

                // Add category
                var entityEntry = await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                // Return the added category with generated ID
                return entityEntry.Entity;
            }
            catch (Exception ex) when (!(ex is ArgumentNullException || ex is InvalidOperationException))
            {
                throw new InvalidOperationException("Error adding new category", ex);
            }
        }

        // Update category
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                if (category == null)
                    throw new ArgumentNullException(nameof(category));

                if (category.CategoryID <= 0)
                    return false;

                // Check if category exists
                var existingCategory = await _context.Categories
                    .Include(c => c.Translations)
                    .FirstOrDefaultAsync(c => c.CategoryID == category.CategoryID);

                if (existingCategory == null)
                    return false;

                // Validate slug uniqueness (exclude current category)
                if (!string.IsNullOrWhiteSpace(category.Slug))
                {
                    var slugExists = await _context.Categories
                        .AsNoTracking()
                        .AnyAsync(c => c.Slug == category.Slug.ToLower().Trim() && c.CategoryID != category.CategoryID);

                    if (slugExists)
                        throw new InvalidOperationException($"Category with slug '{category.Slug}' already exists");
                }

                // Update basic properties
                existingCategory.Slug = category.Slug.ToLower().Trim();

                // Update translations
                if (category.Translations != null && category.Translations.Any())
                {
                    // Remove existing translations
                    _context.CategoriesTranslations.RemoveRange(existingCategory.Translations);

                    // Add new translations
                    foreach (var translation in category.Translations)
                    {
                        translation.CategoryID = existingCategory.CategoryID;
                        _context.CategoriesTranslations.Add(translation);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) when (!(ex is ArgumentNullException || ex is InvalidOperationException))
            {
                throw new InvalidOperationException($"Error updating category with ID: {category?.CategoryID}", ex);
            }
        }

        // Delete category
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return false;

                var category = await _context.Categories
                    .Include(c => c.Translations)
                    .Include(c => c.Products) // Check for related products
                    .FirstOrDefaultAsync(c => c.CategoryID == id);

                if (category == null)
                    return false;

                // Check if category has related products
                if (category.Products != null && category.Products.Any())
                {
                    throw new InvalidOperationException("Cannot delete category that has associated products");
                }

                // Remove translations first
                if (category.Translations != null && category.Translations.Any())
                {
                    _context.CategoriesTranslations.RemoveRange(category.Translations);
                }

                // Remove category
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                throw new InvalidOperationException($"Error deleting category with ID: {id}", ex);
            }
        }

        // Check if category exists
        public async Task<bool> CategoryExistsAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return false;

                return await _context.Categories
                    .AsNoTracking()
                    .AnyAsync(c => c.CategoryID == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking category existence with ID: {id}", ex);
            }
        }

        // Check if slug exists
        public async Task<bool> SlugExistsAsync(string slug, int? excludeCategoryId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(slug))
                    return false;

                var normalizedSlug = slug.ToLower().Trim();
                var query = _context.Categories.AsNoTracking().AsQueryable();

                if (excludeCategoryId.HasValue)
                    query = query.Where(c => c.CategoryID != excludeCategoryId.Value);

                return await query.AnyAsync(c => c.Slug == normalizedSlug);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking slug existence: {slug}", ex);
            }
        }

        // Get categories count
        public async Task<int> GetCategoriesCountAsync(Expression<Func<Category, bool>>? filter = null)
        {
            try
            {
                var query = _context.Categories.AsNoTracking().AsQueryable();

                if (filter != null)
                    query = query.Where(filter);

                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error getting categories count", ex);
            }
        }

        // Get paginated categories
        public async Task<IEnumerable<Category>> GetPaginatedCategoriesAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<Category, bool>>? filter = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                var query = _context.Categories
                    .Include(c => c.Translations)
                    .AsNoTracking()
                    .AsQueryable();

                if (filter != null)
                    query = query.Where(filter);

                if (orderBy != null)
                    query = orderBy(query);
                else
                    query = query.OrderBy(c => c.CategoryID);

                return await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error getting paginated categories", ex);
            }
        }
    }
}