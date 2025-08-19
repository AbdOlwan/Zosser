using DAL.Context;
using DAL.Entities.Models.ProductModels;
using DAL.Repositories.Interfaces.Product_Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTOs.ProductDTOs;
using Shared.Interfaces;
using Shared.Utils;
using System.Diagnostics;

namespace DAL.Repositories.Implementations.Products_Implementaion;

public class ProductRepo : IProductRepo
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductRepo> _logger;
    private readonly ICurrentUserService _currentUserService;

    public ProductRepo(
        ApplicationDbContext context,
        ILogger<ProductRepo> logger,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        using var activity = ActivitySourceProvider.StartActivity();
        try
        {
            var userId = _currentUserService.GetCurrentUserId();
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedBy = userId;
            product.LastModifiedAt = DateTime.UtcNow;
            product.LastModifiedBy = userId;

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product: {ProductName}", product.Name);
            throw new ApplicationException("Failed to add product", ex);
        }
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        using var activity = ActivitySourceProvider.StartActivity();
        try
        {
            product.LastModifiedAt = DateTime.UtcNow;
            product.LastModifiedBy = _currentUserService.GetCurrentUserId();

            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error updating product ID: {ProductId}", product.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product ID: {ProductId}", product.Id);
            throw new ApplicationException($"Failed to update product {product.Id}", ex);
        }
    }

    public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        using var activity = ActivitySourceProvider.StartActivity();
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product not found for deletion: {ProductId}", id);
                return;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product ID: {ProductId}", id);
            throw new ApplicationException($"Failed to delete product {id}", ex);
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id, bool track = false)
    {
        var query = _context.Products
            .Include(p=> p.Category)
            .Include(p => p.Brand)
            .Include(p => p.FAQs)
            .Include(p => p.Images)

            .AsQueryable();

        if (!track)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        using var activity = ActivitySourceProvider.StartActivity();
        try
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all products");
            throw new ApplicationException("Failed to get products", ex);
        }
    }

    public async Task<bool> ProductExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        using var activity = ActivitySourceProvider.StartActivity();
        try
        {
            return await _context.Products
                .AnyAsync(p => p.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking product existence ID: {ProductId}", id);
            throw new ApplicationException($"Failed to check product existence {id}", ex);
        }
    }

    public async Task<(IEnumerable<ProductCardDto> products, int totalCount)> GetProductCardsPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        using var activity = ActivitySourceProvider.StartActivity();
        try
        {
            var query = _context.Products
                .AsNoTracking()
                .Select(p => new ProductCardDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CashbackAmount = p.Cashback,
                    BrandName = p.Brand.Name,
                    PrimaryImageUrl = p.Images
                        .Where(img => img.IsPrimary)
                        .Select(img => img.Url)
                        .FirstOrDefault() ?? p.Images.Select(img => img.Url).FirstOrDefault()!
                });

            int totalCount = await query.CountAsync(cancellationToken);
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (products, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product cards");
            throw new ApplicationException("Failed to get product cards", ex);
        }
    }
}