using DAL.Entities.Models.PaymentsModels.WalletModels;

namespace DAL.Repositories.Interfaces.Payments.Wallets
{
    public interface IWalletTransactionRepo
    {
        Task AddAsync(WalletTransaction transaction, CancellationToken cancellationToken = default);
        Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(int walletId, CancellationToken cancellationToken = default);
    }
}
