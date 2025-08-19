using DAL.Context;
using DAL.Entities.Models.Shipment;
using DAL.Repositories.Interfaces.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Shared.Utils;

namespace DAL.Repositories.Implementations.Shipments
{
    public class DeliveryAgentRepo : IDeliveryAgentRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeliveryAgentRepo> _logger;
        private readonly ICurrentUserService _currentUserService;

        public DeliveryAgentRepo(
            ApplicationDbContext context,
            ILogger<DeliveryAgentRepo> logger,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<DeliveryAgent> CreateDeliveryAgentAsync(DeliveryAgent deliveryAgent, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var userId = _currentUserService.GetCurrentUserId();
                deliveryAgent.CreatedAt = DateTime.UtcNow;
                deliveryAgent.CreatedBy = userId;
                deliveryAgent.LastModifiedAt = DateTime.UtcNow;
                deliveryAgent.LastModifiedBy = userId;

                await _context.DeliveryAgents.AddAsync(deliveryAgent, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return deliveryAgent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating delivery agent: {DeliveryAgentName}", deliveryAgent.Name);
                throw new ApplicationException("Failed to create delivery agent", ex);
            }
        }

        public async Task UpdateDeliveryAgentAsync(DeliveryAgent deliveryAgent, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                deliveryAgent.LastModifiedAt = DateTime.UtcNow;
                deliveryAgent.LastModifiedBy = _currentUserService.GetCurrentUserId();

                _context.DeliveryAgents.Update(deliveryAgent);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating delivery agent ID: {DeliveryAgentId}", deliveryAgent.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery agent ID: {DeliveryAgentId}", deliveryAgent.Id);
                throw new ApplicationException($"Failed to update delivery agent {deliveryAgent.Id}", ex);
            }
        }

        public async Task DeleteDeliveryAgentAsync(int deliveryAgentId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var deliveryAgent = await _context.DeliveryAgents
                    .FirstOrDefaultAsync(da => da.Id == deliveryAgentId, cancellationToken);

                if (deliveryAgent == null)
                {
                    _logger.LogWarning("Delivery agent not found for deletion: {DeliveryAgentId}", deliveryAgentId);
                    return;
                }

                // منع حذف المندوب إذا كانت لديه تحصيلات مرتبطة
                var hasCollections = await _context.PaymentCollections.AnyAsync(pc => pc.DeliveryAgentId == deliveryAgentId, cancellationToken);
                if (hasCollections)
                {
                    _logger.LogWarning($"Attempted to delete delivery agent {deliveryAgentId} with existing payment collections. Deletion not allowed.");
                    throw new InvalidOperationException($"Cannot delete delivery agent {deliveryAgentId} as there are existing payment collections associated with them.");
                }

                _context.DeliveryAgents.Remove(deliveryAgent);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting delivery agent ID: {DeliveryAgentId}", deliveryAgentId);
                throw new ApplicationException($"Failed to delete delivery agent {deliveryAgentId}", ex);
            }
        }

        public async Task<DeliveryAgent?> GetDeliveryAgentByIdAsync(int deliveryAgentId, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.DeliveryAgents.AsQueryable();
            if (!track)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(da => da.Id == deliveryAgentId, cancellationToken);
        }

        public async Task<IEnumerable<DeliveryAgent>> GetAllDeliveryAgentsAsync(CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            return await _context.DeliveryAgents.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<bool> DeliveryAgentExistsAsync(int deliveryAgentId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            return await _context.DeliveryAgents.AnyAsync(da => da.Id == deliveryAgentId, cancellationToken);
        }

        public async Task<DeliveryAgent?> GetDeliveryAgentByNameAsync(string name, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.DeliveryAgents.AsQueryable();
            if (!track)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(da => da.Name == name, cancellationToken);
        }

        public async Task<IEnumerable<DeliveryAgent>> GetActiveDeliveryAgentsAsync(CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            return await _context.DeliveryAgents.AsNoTracking()
                .Where(da => da.IsActive)
                .ToListAsync(cancellationToken);
        }
    }
}
