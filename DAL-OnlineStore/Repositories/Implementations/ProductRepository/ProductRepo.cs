using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        private readonly string _defaultCulture = "ar"; // Arabic is the default language

        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Product>> GetAllProductsAsync(string culture, int page, int limit)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            var query = _context.Products
                .Include(p => p.brand!)
                    .ThenInclude(b => b.BrandTranslations)
                .Include(p => p.category!)
                    .ThenInclude(c => c.Translations)
                .Include(p => p.type)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .Where(p => p.QuantityInStock > 0)
                .AsNoTracking();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = limit,
                TotalItems = totalItems
            };
        }


        public async Task<Product?> GetProductByIdAsync(int id, string? culture)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            return await _context.Products
                .Include(p => p.brand)
                .Include(p => p.brand!)
                .ThenInclude(b => b.BrandTranslations)
                .Include(p => p.category)
                .Include(p => p.category!)
                .ThenInclude(c => c.Translations)
                .Include(p => p.type)
                .Include(p => p.Images)
                .Include(p => p.productSpecifications)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product?> GetProductBySlugAsync(string slug, string culture)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            return await _context.Products
                .Include(p => p.brand)
                .Include(p => p.category)
                .Include(p => p.type)
                .Include(p => p.Images)
                .Include(p => p.productSpecifications)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<PagedResult<Product>> GetProductsByCategoryIdAsync(int categoryId, string culture, int page, int limit)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            var query = _context.Products
                .Where(p => p.CategoryId == categoryId && p.QuantityInStock > 0)
                .Include(p => p.brand!).ThenInclude(b=>b.BrandTranslations)
                .Include(p=>p.category!).ThenInclude(c=>c.Translations)
                .Include(p=>p.type!)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .AsNoTracking();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)limit);

            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            return new PagedResult<Product>
            {
                Items = items,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = limit,            // ✅ هنا بتحدد عدد العناصر في كل صفحة
                TotalItems = totalItems      // ✅ وهنا العدد الكلي لكل المنتجات
            };
        }


        public async Task<PagedResult<Product>> GetProductsByBrandIdAsync(int brandId, string culture, int page, int limit)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            var query = _context.Products
                .Where(p => p.Brand_ID == brandId && p.QuantityInStock > 0)
                .Include(p => p.category!).ThenInclude(c=> c.Translations)  
                .Include(p => p.type!)
                .Include(p=>p.brand!).ThenInclude(b=>b.BrandTranslations)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)limit);

            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            return new PagedResult<Product>
            {
                Items = items,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = limit,            // ✅ هنا بتحدد عدد العناصر في كل صفحة
                TotalItems = totalCount      // ✅ وهنا العدد الكلي لكل المنتجات
            };

        }

        public async Task<PagedResult<Product>> GetProductsByTypeIdAsync(int typeId, string culture, int page, int limit)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            var query =   _context.Products
                .Where(p => p.TypeId == typeId && p.QuantityInStock > 0)
                .Include(p => p.brand!).ThenInclude(b=>b.BrandTranslations)
                .Include(p => p.category!).ThenInclude(c=>c.Translations)
                .Include(p => p.type)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .AsNoTracking();


            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)limit);

            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            return new PagedResult<Product>
            {
                Items = items,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = limit,            
                TotalItems = totalCount      
            };

        }

        public async Task<IEnumerable<Product>> FindProductsAsync(Expression<Func<Product, bool>> predicate, string culture)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            return await _context.Products
                .Where(predicate)
                .Include(p => p.brand)
                .Include(p => p.category)
                .Include(p => p.type)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            // Set created and updated timestamps
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            // Update the timestamp
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task  DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
          
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == id);
        }

        public async Task<bool> ProductExistsBySlugAsync(string slug)
        {
            return await _context.Products.AnyAsync(p => p.Slug == slug);
        }
    }
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }

}

