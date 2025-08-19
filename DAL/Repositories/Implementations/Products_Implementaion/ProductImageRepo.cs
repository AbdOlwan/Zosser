using DAL.Context;
using DAL.Entities.Models.ProductModels;
using DAL.Repositories.Interfaces.Product_Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Shared.Utils;

namespace DAL.Repositories.Implementations.Products_Implementaion
{
    public class ProductImageRepo : IProductImageRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductImageRepo> _logger;
        private readonly ICurrentUserService _currentUserService;

        public ProductImageRepo(
            ApplicationDbContext context,
            ILogger<ProductImageRepo> logger,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<ProductImage> AddImageAsync(ProductImage image, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var userId = _currentUserService.GetCurrentUserId();
                image.CreatedAt = DateTime.UtcNow;
                image.CreatedBy = userId;
                image.LastModifiedAt = DateTime.UtcNow;
                image.LastModifiedBy = userId;

                await _context.ProductImages.AddAsync(image, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return image;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding image for product ID: {ProductId}", image.ProductId);
                throw new ApplicationException("Failed to add image", ex);
            }
        }

        public async Task UpdateImageAsync(ProductImage image, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                image.LastModifiedAt = DateTime.UtcNow;
                image.LastModifiedBy = _currentUserService.GetCurrentUserId();

                _context.ProductImages.Update(image);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating image ID: {ImageId}", image.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating image ID: {ImageId}", image.Id);
                throw new ApplicationException($"Failed to update image {image.Id}", ex);
            }
        }

        public async Task DeleteImageAsync(int id, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var image = await _context.ProductImages.FindAsync(id);
                if (image == null)
                {
                    _logger.LogWarning("Image not found for deletion: {ImageId}", id);
                    return;
                }

                _context.ProductImages.Remove(image);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image ID: {ImageId}", id);
                throw new ApplicationException($"Failed to delete image {id}", ex);
            }
        }

        public async Task<ProductImage?> GetImageByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.ProductImages
                    .AsNoTracking()
                    .Include(img => img.Product)
                    .FirstOrDefaultAsync(img => img.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting image by ID: {ImageId}", id);
                throw new ApplicationException($"Failed to get image {id}", ex);
            }
        }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(
            int productId,
            CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.ProductImages
                    .AsNoTracking()
                    .Where(img => img.ProductId == productId)
                    .OrderByDescending(img => img.IsPrimary)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting images for product ID: {ProductId}", productId);
                throw new ApplicationException($"Failed to get images for product {productId}", ex);
            }
        }
    }
}