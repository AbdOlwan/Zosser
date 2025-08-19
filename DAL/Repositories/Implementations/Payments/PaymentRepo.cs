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
    public class PaymentRepo : IPaymentRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentRepo> _logger;
        private readonly ICurrentUserService _currentUserService;

        public PaymentRepo(
            ApplicationDbContext context,
            ILogger<PaymentRepo> logger,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var userId = _currentUserService.GetCurrentUserId();
                payment.CreatedAt = DateTime.UtcNow;
                payment.CreatedBy = userId;
                payment.LastModifiedAt = DateTime.UtcNow;
                payment.LastModifiedBy = userId;

                await _context.Payments.AddAsync(payment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment for order ID: {OrderId}", payment.OrderId);
                throw new ApplicationException("Failed to create payment", ex);
            }
        }

        public async Task UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                payment.LastModifiedAt = DateTime.UtcNow;
                payment.LastModifiedBy = _currentUserService.GetCurrentUserId();

                _context.Payments.Update(payment);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating payment ID: {PaymentId}", payment.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment ID: {PaymentId}", payment.Id);
                throw new ApplicationException($"Failed to update payment {payment.Id}", ex);
            }
        }

        public async Task DeletePaymentAsync(int paymentId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);

                if (payment == null)
                {
                    _logger.LogWarning("Payment not found for deletion: {PaymentId}", paymentId);
                    return;
                }

                if (payment.Status == PaymentStatus.Completed || payment.Status == PaymentStatus.Refunded || payment.Status == PaymentStatus.CollectedFromCustomer)
                {
                    _logger.LogWarning($"Attempted to delete a completed, refunded, or collected payment: {paymentId}. Deletion not allowed.");
                    throw new InvalidOperationException($"Cannot delete a payment with status {payment.Status}.");
                }

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment ID: {PaymentId}", paymentId);
                throw new ApplicationException($"Failed to delete payment {paymentId}", ex);
            }
        }

        public async Task<Payment?> GetPaymentByIdAsync(int paymentId, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.Payments
                .Include(p => p.Order)
                .AsQueryable();

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);
        }

        public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.Payments
                .Include(p => p.Order)
                .AsQueryable();

            if (!track)
            {
                query = query.AsNoTracking();
            }
            // بما أن الدفع مرة واحدة لكل طلب، نستخدم FirstOrDefault
            return await query.FirstOrDefaultAsync(p => p.OrderId == orderId, cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.Payments
                    .AsNoTracking()
                    .Include(p => p.Order)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all payments");
                throw new ApplicationException("Failed to get all payments", ex);
            }
        }

        public async Task<bool> PaymentExistsAsync(int paymentId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.Payments
                    .AnyAsync(p => p.Id == paymentId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking payment existence ID: {PaymentId}", paymentId);
                throw new ApplicationException($"Failed to check payment existence {paymentId}", ex);
            }
        }

        public async Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.Payments
                .Include(p => p.Order)
                .AsQueryable();

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(p => p.TransactionId == transactionId, cancellationToken);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.Payments
                    .AsNoTracking()
                    .Where(p => p.Order.CustomerId == customerId)
                    .Include(p => p.Order)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments for customer ID: {CustomerId}", customerId);
                throw new ApplicationException($"Failed to get payments for customer {customerId}", ex);
            }
        }
    }
}
