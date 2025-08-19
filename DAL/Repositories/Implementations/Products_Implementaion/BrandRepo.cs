using DAL.Context;
using DAL.Entities.Models.ProductModels;
using DAL.Repositories.Interfaces.Product_Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Shared.Utils;

namespace DAL.Repositories.Implementations.Products_Implementaion
{
    public class BrandRepo : IBrandRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BrandRepo> _logger;
        private readonly ICurrentUserService _currentUserService;

        public BrandRepo(
            ApplicationDbContext context,
            ILogger<BrandRepo> logger,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<Brand> AddBrandAsync(Brand brand, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var userId = _currentUserService.GetCurrentUserId();
                brand.CreatedAt = DateTime.UtcNow;
                brand.CreatedBy = userId;
                brand.LastModifiedAt = DateTime.UtcNow;
                brand.LastModifiedBy = userId;

                await _context.Brands.AddAsync(brand, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return brand;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding brand: {BrandName}", brand.Name);
                throw new ApplicationException("Failed to add brand", ex);
            }
        }

        public async Task UpdateBrandAsync(Brand brand, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                brand.LastModifiedAt = DateTime.UtcNow;
                brand.LastModifiedBy = _currentUserService.GetCurrentUserId();

                _context.Brands.Update(brand);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                _logger.LogWarning("Unique constraint violation updating brand ID: {BrandId}", brand.Id);
                throw new ApplicationException("Brand name must be unique", dbEx);
            }
            // BrandRepo.cs - UpdateBrandAsync method
            catch (Exception ex)
            {
                // ADD THIS LINE TO LOG THE INNER EXCEPTION:
                _logger.LogError(ex.InnerException ?? ex, "Error updating brand ID: {BrandId}", brand.Id);

                throw new ApplicationException($"Failed to update brand {brand.Id}", ex);
            }
        }

        public async Task DeleteBrandAsync(int id, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var brand = await _context.Brands.FindAsync(id);
                if (brand == null)
                {
                    _logger.LogWarning("Brand not found for deletion: {BrandId}", id);
                    return;
                }

                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting brand ID: {BrandId}", id);
                throw new ApplicationException($"Failed to delete brand {id}", ex);
            }
        }

        public async Task<Brand?> GetBrandByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.Brands
                    .AsNoTracking()
                    .Include(b => b.Products)
                    .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting brand by ID: {BrandId}", id);
                throw new ApplicationException($"Failed to get brand {id}", ex);
            }
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.Brands
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all brands");
                throw new ApplicationException("Failed to get brands", ex);
            }
        }
    }
}