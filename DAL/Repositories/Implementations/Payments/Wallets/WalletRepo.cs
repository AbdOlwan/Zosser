using DAL.Context;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using DAL.Repositories.Interfaces.Payments.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace DAL.Repositories.Implementations.Payments.Wallets
{
    public class WalletRepo : IWalletRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WalletRepo> _logger;

        public WalletRepo(ApplicationDbContext context, ILogger<WalletRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Wallet> CreateAsync(Wallet wallet, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Wallets.AddAsync(wallet, cancellationToken);
                return wallet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating wallet for customer ID: {CustomerId}", wallet.CustomerId);
                throw new ApplicationException("Failed to create wallet", ex);
            }
        }

        public async Task<Wallet?> GetByCustomerIdAsync(int customerId, bool track = false, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await _context.Wallets
                    .AsNoTracking()
                    .Include(b => b.Customer!)
                    .ThenInclude(c=> c.User)

                    .FirstOrDefaultAsync(b => b.CustomerId == customerId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Wallet by ID: {customerId}", customerId);
                throw new ApplicationException($"Failed to get wallet {customerId}", ex);
            }
        }

        public async Task<Wallet?> GetByIdAsync(int walletId, bool track = false, CancellationToken cancellationToken = default)
        {
            var query = _context.Wallets.AsQueryable();
            if (!track)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(w => w.Id == walletId, cancellationToken);
        }

        public void Update(Wallet wallet)
        {
            // هذا التابع لا يحفظ في قاعدة البيانات مباشرة
            // بل يغير حالة الكيان إلى "معدل" ليتم حفظه لاحقاً
            _context.Wallets.Update(wallet);
        }
    }
}
