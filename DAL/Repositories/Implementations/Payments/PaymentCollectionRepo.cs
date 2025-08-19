using DAL.Context;
using DAL.Entities.Models.PaymentsModels;
using DAL.Repositories.Interfaces.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTOs.Payments;
using Shared.Interfaces;
using Shared.Utils;

namespace DAL.Repositories.Implementations.Payments
{
    public class PaymentCollectionRepo : IPaymentCollectionRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentCollectionRepo> _logger;
        private readonly ICurrentUserService _currentUserService;

        public PaymentCollectionRepo(
            ApplicationDbContext context,
            ILogger<PaymentCollectionRepo> logger,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<PaymentCollection> CreatePaymentCollectionAsync(PaymentCollection paymentCollection, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var userId = _currentUserService.GetCurrentUserId();
                paymentCollection.CreatedAt = DateTime.UtcNow;
                paymentCollection.CreatedBy = userId;
                paymentCollection.LastModifiedAt = DateTime.UtcNow;
                paymentCollection.LastModifiedBy = userId;

                await _context.PaymentCollections.AddAsync(paymentCollection, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                // تحديث حالة الدفعة المرتبطة بها بعد التحصيل الناجح
                var payment = await _context.Payments.FindAsync(paymentCollection.PaymentId);
                if (payment != null && payment.Method == PaymentMethod.CashOnDelivery && payment.Status == PaymentStatus.Pending)
                {
                    payment.Status = PaymentStatus.CollectedFromCustomer;
                    payment.LastModifiedAt = DateTime.UtcNow;
                    payment.LastModifiedBy = userId;
                    _context.Payments.Update(payment);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return paymentCollection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment collection for payment ID: {PaymentId}", paymentCollection.PaymentId);
                throw new ApplicationException("Failed to create payment collection", ex);
            }
        }

        public async Task UpdatePaymentCollectionAsync(PaymentCollection paymentCollection, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                paymentCollection.LastModifiedAt = DateTime.UtcNow;
                paymentCollection.LastModifiedBy = _currentUserService.GetCurrentUserId();

                _context.PaymentCollections.Update(paymentCollection);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating payment collection ID: {PaymentCollectionId}", paymentCollection.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment collection ID: {PaymentCollectionId}", paymentCollection.Id);
                throw new ApplicationException($"Failed to update payment collection {paymentCollection.Id}", ex);
            }
        }

        public async Task DeletePaymentCollectionAsync(int paymentCollectionId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var paymentCollection = await _context.PaymentCollections
                    .FirstOrDefaultAsync(pc => pc.Id == paymentCollectionId, cancellationToken);

                if (paymentCollection == null)
                {
                    _logger.LogWarning("Payment collection not found for deletion: {PaymentCollectionId}", paymentCollectionId);
                    return;
                }

                // يمكنك إضافة منطق لمنع الحذف إذا تم التحقق من التحصيل
                if (paymentCollection.VerifiedAt.HasValue) // Corrected from HasValue() to HasValue
                {
                    _logger.LogWarning($"Attempted to delete a verified payment collection: {paymentCollectionId}. Deletion not allowed.");
                    throw new InvalidOperationException("Cannot delete a verified payment collection.");
                }

                _context.PaymentCollections.Remove(paymentCollection);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment collection ID: {PaymentCollectionId}", paymentCollectionId);
                throw new ApplicationException($"Failed to delete payment collection {paymentCollectionId}", ex);
            }
        }

        public async Task<PaymentCollection?> GetPaymentCollectionByIdAsync(int paymentCollectionId, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.PaymentCollections
                .Include(pc => pc.Payment)
                    .ThenInclude(p => p.Order)
                .Include(pc => pc.DeliveryAgent) // Changed to DeliveryAgent
                .AsQueryable();

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(pc => pc.Id == paymentCollectionId, cancellationToken);
        }

        public async Task<PaymentCollection?> GetPaymentCollectionByPaymentIdAsync(int paymentId, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.PaymentCollections
                .Include(pc => pc.Payment)
                    .ThenInclude(p => p.Order)
                .Include(pc => pc.DeliveryAgent)
                .AsQueryable();

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(pc => pc.PaymentId == paymentId, cancellationToken);
        }

        public async Task<IEnumerable<PaymentCollection>> GetCollectionsByDeliveryAgentIdAsync(int deliveryAgentId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            return await _context.PaymentCollections
                .AsNoTracking()
                .Include(pc => pc.Payment)
                    .ThenInclude(p => p.Order)
                .Include(pc => pc.DeliveryAgent)
                .Where(pc => pc.DeliveryAgentId == deliveryAgentId)
                .OrderByDescending(pc => pc.CollectedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PaymentCollection>> GetCollectionsByVerificationStatusAsync(bool isVerified, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            // Explicitly declare query as IQueryable<PaymentCollection> to avoid CS0266
            IQueryable<PaymentCollection> query = _context.PaymentCollections.AsNoTracking()
                .Include(pc => pc.Payment)
                    .ThenInclude(p => p.Order)
                .Include(pc => pc.DeliveryAgent);

            if (isVerified)
            {
                query = query.Where(pc => pc.VerifiedAt.HasValue); // Corrected from HasValue() to HasValue
            }
            else
            {
                query = query.Where(pc => !pc.VerifiedAt.HasValue); // Corrected from HasValue() to HasValue
            }

            return await query.OrderByDescending(pc => pc.CollectedAt).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PaymentCollection>> GetAllPaymentCollectionsAsync(CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            return await _context.PaymentCollections
                .AsNoTracking()
                .Include(pc => pc.Payment)
                    .ThenInclude(p => p.Order)
                .Include(pc => pc.DeliveryAgent)
                .OrderByDescending(pc => pc.CollectedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> PaymentCollectionExistsAsync(int paymentCollectionId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            return await _context.PaymentCollections.AnyAsync(pc => pc.Id == paymentCollectionId, cancellationToken);
        }
    }
}
