using DAL.Entities.Models.PaymentsModels.WalletModels;
using Shared.DTOs.Payments.Wallets;

namespace DAL.Repositories.Interfaces.Payments.Wallets
{
    public interface IWithdrawalRequestRepo
    {
        Task<WithdrawalRequest?> GetByIdAsync(int requestId, bool track = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<WithdrawalRequest>> GetByWalletIdAsync(int walletId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<WithdrawalRequest>> GetByStatusAsync(WithdrawalStatus status, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task AddAsync(WithdrawalRequest request, CancellationToken cancellationToken = default);
        void Update(WithdrawalRequest request);

        Task<int> GetTotalCountAsync(int walletId, CancellationToken cancellationToken = default);
        Task<int> GetTotalCountByStatusAsync(WithdrawalStatus status, CancellationToken cancellationToken = default);
    }
}
