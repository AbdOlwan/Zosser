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

        public async Task<IEnumerable<Product>> GetAllProductsAsync(string culture)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            return await _context.Products
                .Include(p=>p.brand)
                .Include(p => p.brand!).ThenInclude(B => B.BrandTranslations)
                .Include(p=>p.category)
                .Include(p => p.category!).ThenInclude(B => B.Translations)
                .Include(p => p.type)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .Where(p=>p.QuantityInStock >0)
                .AsNoTracking()
                .ToListAsync();
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

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId, string culture)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.brand)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandIdAsync(int brandId, string culture)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            return await _context.Products
                .Where(p => p.Brand_ID == brandId)
                .Include(p => p.category)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByTypeIdAsync(int typeId, string culture)
        {
            culture = !string.IsNullOrEmpty(culture) ? culture : _defaultCulture;

            return await _context.Products
                .Where(p => p.TypeId == typeId)
                .Include(p => p.brand)
                .Include(p => p.category)
                .Include(p => p.Images)
                .Include(p => p.productTranslations.Where(pt => pt.Culture == culture))
                .AsNoTracking()
                .ToListAsync();
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
}

