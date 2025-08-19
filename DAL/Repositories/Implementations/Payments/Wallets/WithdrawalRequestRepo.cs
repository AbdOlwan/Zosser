using DAL.Context;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using DAL.Repositories.Interfaces.Payments.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DTOs.Payments.Wallets;

namespace DAL.Repositories.Implementations.Payments.Wallets
{
    public class WithdrawalRequestRepo : IWithdrawalRequestRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WithdrawalRequestRepo> _logger;

        public WithdrawalRequestRepo(ApplicationDbContext context, ILogger<WithdrawalRequestRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(WithdrawalRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.WithdrawalRequests.AddAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating withdrawal request for wallet ID: {WalletId}", request.WalletId);
                throw new ApplicationException("Failed to create withdrawal request", ex);
            }
        }

        public async Task<WithdrawalRequest?> GetByIdAsync(int requestId, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.WithdrawalRequests
                .Include(r => r.Wallet)
                    .ThenInclude(w => w.Customer)
                        .ThenInclude(c => c.User) // تأكد من تضمين المستخدم
                .Include(r => r.ProcessedByUser)
                .AsQueryable();

            if (!track) query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);
        }

        public async Task<IEnumerable<WithdrawalRequest>> GetByStatusAsync(
            WithdrawalStatus status,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            return await _context.WithdrawalRequests
                .AsNoTracking()
                .Include(r => r.Wallet)
                    .ThenInclude(w => w.Customer)
                        .ThenInclude(c => c.User) // المسار الكامل للعميل والمستخدم
                .Include(r => r.ProcessedByUser) // تضمين معالج الطلب
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.RequestDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<WithdrawalRequest>> GetByWalletIdAsync(int walletId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.WithdrawalRequests
               .AsNoTracking()
               .Where(r => r.WalletId == walletId)
               .OrderByDescending(r => r.RequestDate)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync(cancellationToken);
        }

        public void Update(WithdrawalRequest request)
        {
            _context.WithdrawalRequests.Update(request);
        }


        public async Task<int> GetTotalCountAsync(int walletId, CancellationToken cancellationToken = default)
        {
            return await _context.WithdrawalRequests
                .CountAsync(r => r.WalletId == walletId, cancellationToken);
        }

        public async Task<int> GetTotalCountByStatusAsync(WithdrawalStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.WithdrawalRequests
                .CountAsync(r => r.Status == status, cancellationToken);
        }
    }
}
