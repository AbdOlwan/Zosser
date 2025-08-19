using DAL.Context;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using DAL.Repositories.Interfaces.Payments.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories.Implementations.Payments.Wallets
{
    public class WalletTransactionRepo : IWalletTransactionRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WalletTransactionRepo> _logger;

        public WalletTransactionRepo(ApplicationDbContext context, ILogger<WalletTransactionRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(WalletTransaction transaction, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.WalletTransactions.AddAsync(transaction, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding transaction for wallet ID: {WalletId}", transaction.WalletId);
                throw new ApplicationException("Failed to add transaction", ex);
            }
        }

        public async Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(int walletId, CancellationToken cancellationToken = default)
        {
            return await _context.WalletTransactions
                .AsNoTracking()
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.TransactionDate)

                .ToListAsync(cancellationToken);
        }
    }
}
