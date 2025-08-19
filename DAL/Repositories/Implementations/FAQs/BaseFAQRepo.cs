using DAL.Context;
using DAL.Entities.Models.FaqModels;
using DAL.Repositories.Interfaces.FAQs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace DAL.Repositories.Implementations.FAQs
{
    public abstract class BaseFAQRepo<T> : IBaseFAQRepo<T> where T : BaseFAQ
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ILogger<BaseFAQRepo<T>> _logger;
        protected readonly ICurrentUserService _currentUserService;
        protected abstract DbSet<T> DbSet { get; }

        protected BaseFAQRepo(
            ApplicationDbContext context,
            ILogger<BaseFAQRepo<T>> logger,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                var userId = _currentUserService.GetCurrentUserId();

                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = userId;
                entity.LastModifiedAt = DateTime.UtcNow;
                entity.LastModifiedBy = userId;

                await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding {typeof(T).Name}: {entity.Question}");
                throw new ApplicationException($"Failed to add {typeof(T).Name}", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                entity.LastModifiedAt = DateTime.UtcNow;
                entity.LastModifiedBy = _currentUserService.GetCurrentUserId();

                DbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating {typeof(T).Name} ID: {entity.Id}");
                throw new ApplicationException($"Failed to update {typeof(T).Name} ID: {entity.Id}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await DbSet.FindAsync(id);
                if (entity != null)
                {
                    DbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting {typeof(T).Name} ID: {id}");
                throw new ApplicationException($"Failed to delete {typeof(T).Name} ID: {id}", ex);
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await DbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting {typeof(T).Name} by ID: {id}");
                throw new ApplicationException($"Failed to get {typeof(T).Name} ID: {id}", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await DbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting all {typeof(T).Name}s");
                throw new ApplicationException($"Failed to get all {typeof(T).Name}s", ex);
            }
        }
    }
}
