using DAL.Entities.Models.PaymentsModels.WalletModels;
using Shared.Constants;
using Shared.DTOs.Payments.Wallets;

namespace BLL.Services.Interfaces.Payments.Wallets
{
    public interface IWalletService
    {

            Task CreateWalletAsync(int customerId);

        Task<WalletResponseDto> GetWalletByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
        Task<WithdrawalRequestResponseDto> CreateWithdrawalRequestAsync(WithdrawalRequestCreateDto requestDto, CancellationToken cancellationToken = default);
        Task<WithdrawalRequestResponseDto> ProcessWithdrawalRequestAsync(int requestId, WithdrawalRequestUpdateDto updateDto, CancellationToken cancellationToken = default);
        Task<PagedResponse<WithdrawalRequestResponseDto>> GetWithdrawalRequestsForCustomerAsync(int customerId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<PagedResponse<WithdrawalRequestResponseDto>> GetWithdrawalRequestsForAdminAsync(WithdrawalStatus status, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task AddFundsAsync(int customerId, decimal amount, string description, int? sourceId = null, string? sourceType = null, CancellationToken cancellationToken = default);
        Task UseWalletForPurchaseAsync(int customerId, decimal amount, int orderId, CancellationToken cancellationToken = default);

        Task<int> GetPendingWithdrawalRequestsCountAsync(CancellationToken cancellationToken = default);
        // This for API tO API
        Task ProcessCashbackBatchAsync(List<CashbackBatchItemDto> cashbackItems, CancellationToken cancellationToken = default);

    }
}
