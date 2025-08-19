using DAL.Entities.Models.PaymentsModels.WalletModels;

namespace DAL.Repositories.Interfaces.Payments.Wallets
{
    public interface IWalletRepo
    {
        Task<Wallet?> GetByIdAsync(int walletId, bool track = false, CancellationToken cancellationToken = default);
        Task<Wallet?> GetByCustomerIdAsync(int customerId, bool track = false, CancellationToken cancellationToken = default);
        Task<Wallet> CreateAsync(Wallet wallet, CancellationToken cancellationToken = default);
        void Update(Wallet wallet); 
    }
}
