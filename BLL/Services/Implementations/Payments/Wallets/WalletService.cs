using BLL.Exceptions;
using BLL.Services.Interfaces.Payments.Wallets;
using DAL.Entities.Models.PaymentsModels.WalletModels;
using DAL.Repositories.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DTOs.Payments.Wallets;
using Shared.Interfaces;
using Shared.Utils;
using System.Threading;

namespace BLL.Services.Implementations.Payments.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<WalletService> _logger;

        public WalletService(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<WalletService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task CreateWalletAsync(int customerId)
        {
            var customer =  await _unitOfWork.Customers.GetCustomerById(customerId);
            if (customer== null)
            {
                throw new NotFoundException("العميل غير موجود");
            }
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId);

            if (wallet == null)
            { wallet = new Wallet
            {
                CustomerId = customerId,
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };
                await _unitOfWork.Wallets.CreateAsync(wallet);
            }
        }

        public async Task<WalletResponseDto> GetWalletByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            // التحقق من الصلاحيات
            //if (!_currentUserService.IsInRole(Roles.Admin) &&
            //    _currentUserService.GetCustomerId() != customerId)
            //{
            //    throw new UnauthorizedAccessException(ValidationMessages.UnauthorizedAccess);
            //}

            // الحصول على المحفظة
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException("المحفظة غير موجودة");

            // الحصول على الحركات المالية (مجدولة)
            var transactions = await _unitOfWork.WalletTransactions.GetByWalletIdAsync( wallet.Id, cancellationToken);

            // التحويل إلى DTO
            var response = wallet.Adapt<WalletResponseDto>();
            response.Transactions = transactions.Adapt<List<WalletTransactionResponseDto>>();

            return response;
        }

        public async Task<WithdrawalRequestResponseDto> CreateWithdrawalRequestAsync( WithdrawalRequestCreateDto requestDto,
            CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            // التحقق من المستخدم الحالي
            var customerId = _currentUserService.GetCustomerId();
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId, track: true, cancellationToken)
                ?? throw new NotFoundException("المحفظة غير موجودة");

            // التحقق من الرصيد الكافي
            if (wallet.Balance < requestDto.Amount)
            {
                throw new BusinessException("الرصيد غير كافي لإتمام عملية السحب");
            }

            // بدء معاملة قاعدة البيانات
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // خصم المبلغ من الرصيد
                wallet.Balance -= requestDto.Amount;

                // تسجيل حركة السحب
                var withdrawalTransaction = new WalletTransaction
                {
                    WalletId = wallet.Id,
                    Amount = requestDto.Amount,
                    Type = TransactionType.Withdrawal,
                    TransactionDate = DateTime.UtcNow,
                    Description = "طلب سحب أموال",
                    SourceType = "WithdrawalRequest"
                };
                await _unitOfWork.WalletTransactions.AddAsync(withdrawalTransaction, cancellationToken);

                // إنشاء طلب السحب
                var withdrawalRequest = requestDto.Adapt<WithdrawalRequest>();
                withdrawalRequest.WalletId = wallet.Id;
                withdrawalRequest.Status = WithdrawalStatus.Pending;
                withdrawalRequest.RequestDate = DateTime.UtcNow;

                await _unitOfWork.WithdrawalRequests.AddAsync(withdrawalRequest, cancellationToken);
                await _unitOfWork.SaveChangesAsync();

                // إرفاق معرف الحركة المالية بطلب السحب
                withdrawalTransaction.SourceId = withdrawalRequest.Id;
                withdrawalTransaction.SourceType = "WithdrawalRequest";
                _unitOfWork.WithdrawalRequests.Update(withdrawalRequest);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return withdrawalRequest.Adapt<WithdrawalRequestResponseDto>();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "فشل في إنشاء طلب سحب");
                throw new ServiceException("فشل في إنشاء طلب سحب", ex);
            }
        }

        public async Task<WithdrawalRequestResponseDto> ProcessWithdrawalRequestAsync(
            int requestId,
            WithdrawalRequestUpdateDto updateDto,
            CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            // التحقق من صلاحيات المسؤول
            //if (!_currentUserService.IsInAnyRole(Roles.Admin, Roles.Manager))
            //{
            //    throw new UnauthorizedAccessException(ValidationMessages.UnauthorizedAccess);
            //}

            var request = await _unitOfWork.WithdrawalRequests.GetByIdAsync(requestId, true, cancellationToken)
                ?? throw new NotFoundException("طلب السحب غير موجود");

            // بدء معاملة قاعدة البيانات
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // تحديث حالة الطلب
                request.Status = (WithdrawalStatus)updateDto.Status;
                request.AdminNotes = updateDto.AdminNotes;
                request.LastUpdateDate = DateTime.UtcNow;
                request.ProcessedByUserId = _currentUserService.UserId;


                var wallet = await _unitOfWork.Wallets.GetByIdAsync(request.WalletId, true, cancellationToken);
                if (wallet != null)
                {


                    if(request.Status == WithdrawalStatus.Approved)
                    {
                        // إذا تمت الموافقة، خصم المبلغ من الرصيد
                        if (wallet.Balance < request.Amount)
                        {
                            throw new BusinessException("الرصيد غير كافي لإتمام عملية السحب");
                        }
                        wallet.Balance -= request.Amount;

                        var withdrawTransaction = new WalletTransaction
                        {
                            WalletId = wallet.Id,
                            Amount = request.Amount,
                            Type = TransactionType.Withdrawal,
                            TransactionDate = DateTime.UtcNow,
                            Description = $"   تمت الموافقة علي طلب رقم  #{request.Id}  " +
                            $"    لتحويل المبلغ علي محفظة رقم #{request.EWalletNumber}     " +
                            $"    {request.WhatsAppNumber} ,وارسال تأكيد علي       ",
                            SourceId = request.Id,
                            SourceType = "WithdrawalRequest"
                        };
                        await _unitOfWork.WalletTransactions.AddAsync(withdrawTransaction, cancellationToken);
                    }
                    else
                    {
                        // إذا تم رفض الطلب، لا نخصم أي شيء
                        if (request.Status != WithdrawalStatus.Rejected &&
                            request.Status != WithdrawalStatus.Cancelled)
                        {
                            throw new BusinessException("حالة الطلب غير صالحة");
                        }
                    }
                }
                _unitOfWork.WithdrawalRequests.Update(request);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return request.Adapt<WithdrawalRequestResponseDto>();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "فشل في معالجة طلب السحب #{RequestId}", requestId);
                throw new ServiceException("فشل في معالجة طلب السحب", ex);
            }
        }

        public async Task<PagedResponse<WithdrawalRequestResponseDto>> GetWithdrawalRequestsForCustomerAsync(
            int customerId,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            // التحقق من الصلاحيات
            if (!_currentUserService.IsInRole(Roles.Admin) &&
                _currentUserService.GetCustomerId() != customerId)
            {
                throw new UnauthorizedAccessException(ValidationMessages.UnauthorizedAccess);
            }

            // الحصول على المحفظة
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException("المحفظة غير موجودة");

            // الحصول على طلبات السحب
            var requests = await _unitOfWork.WithdrawalRequests.GetByWalletIdAsync(
                wallet.Id, pageNumber, pageSize, cancellationToken);

            // التحويل إلى DTO
            var response = new PagedResponse<WithdrawalRequestResponseDto>
            {
                Items = requests.Adapt<List<WithdrawalRequestResponseDto>>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = await _unitOfWork.WithdrawalRequests.GetTotalCountAsync(wallet.Id, cancellationToken)
            };

            return response;
        }

        public async Task<PagedResponse<WithdrawalRequestResponseDto>> GetWithdrawalRequestsForAdminAsync(
            WithdrawalStatus status,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            // التحقق من صلاحيات المسؤول
            //if (!_currentUserService.IsInAnyRole(Roles.Admin, Roles.Manager))
            //{
            //    throw new UnauthorizedAccessException(ValidationMessages.UnauthorizedAccess);
            //}


            // الحصول على طلبات السحب حسب الحالة
            var requests = await _unitOfWork.WithdrawalRequests.GetByStatusAsync(
                status, pageNumber, pageSize, cancellationToken);

            if (requests.Any(r => r.Wallet?.Customer?.User == null))
            {
                _logger.LogWarning("Missing user data in withdrawal requests");
            }

            _logger.LogInformation($"Fetched {requests.Count()} requests");
            foreach (var request in requests)
            {
                _logger.LogInformation($"Request ID: {request.Id}, Customer: {request.Wallet?.Customer?.User?.Name}");
            }

            // التحويل إلى DTO
            var response = new PagedResponse<WithdrawalRequestResponseDto>
            {
                Items = requests.Adapt<List<WithdrawalRequestResponseDto>>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = await _unitOfWork.WithdrawalRequests.GetTotalCountByStatusAsync(status, cancellationToken)
            };

            return response;
        }

        public async Task AddFundsAsync(
            int customerId,
            decimal amount,
            string description,
            int? sourceId = null,
            string? sourceType = null,
            CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            // بدء معاملة قاعدة البيانات
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // الحصول على المحفظة
                var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId, track: true, cancellationToken)
                    ?? throw new NotFoundException("المحفظة غير موجودة");

                // إضافة الرصيد
                wallet.Balance += amount;

                // تسجيل حركة الإيداع
                var transaction = new WalletTransaction
                {
                    WalletId = wallet.Id,
                    Amount = amount,
                    Type = TransactionType.Deposit,
                    TransactionDate = DateTime.UtcNow,
                    Description = description,
                    SourceId = sourceId,
                    SourceType = sourceType
                };
                await _unitOfWork.WalletTransactions.AddAsync(transaction, cancellationToken);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "فشل في إضافة الأموال للمحفظة");
                throw new ServiceException("فشل في إضافة الأموال للمحفظة", ex);
            }
        }


        /// <summary>
        /// يستخدم رصيد المحفظة لإتمام عملية شراء. يتم استدعاؤه داخل Transaction.
        /// </summary>
        /// <param name="customerId">معرف العميل</param>
        /// <param name="amount">المبلغ المطلوب خصمه</param>
        /// <param name="orderId">معرف الطلب لربط الحركة المالية به</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotFoundException">إذا لم يتم العثور على المحفظة</exception>
        /// <exception cref="BusinessException">إذا كان الرصيد غير كافٍ</exception>
        public async Task UseWalletForPurchaseAsync(int customerId, decimal amount, int orderId, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            _logger.LogInformation("Attempting to use wallet for purchase for Customer ID: {CustomerId}, Order ID: {OrderId}, Amount: {Amount}", customerId, orderId, amount);

            // الكود بعد التعديل
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customerId, track: true, cancellationToken) ?? throw new NotFoundException($"محفظة العميل رقم {customerId} غير موجودة.");

            if (wallet.Balance < amount)
            {
                _logger.LogWarning("Insufficient funds for Customer ID: {CustomerId}. Balance: {Balance}, Required: {Amount}", customerId, wallet.Balance, amount);
                throw new BusinessException("الرصيد في المحفظة غير كافٍ لإتمام عملية الشراء.");
            }

            // 1. خصم المبلغ من الرصيد
            wallet.Balance -= amount;

            // 2. إنشاء سجل حركة مالية (Transaction) لتوثيق العملية
            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Type = TransactionType.Purchase, // استخدام النوع الجديد
                TransactionDate = DateTime.UtcNow,
                Description = $"شراء منتج (طلب رقم #{orderId})",
                SourceId = orderId,
                SourceType = "Order"
            };

            await _unitOfWork.WalletTransactions.AddAsync(transaction, cancellationToken);
            _unitOfWork.Wallets.Update(wallet);

            _logger.LogInformation("Successfully processed wallet purchase for Customer ID: {CustomerId}, Order ID: {OrderId}. New balance: {NewBalance}", customerId, orderId, wallet.Balance);

            // ملاحظة: لا نستخدم SaveChangesAsync هنا، لأنها ستتم إدارتها بواسطة الـ Transaction في OrderService
        }


        public async Task<int> GetPendingWithdrawalRequestsCountAsync(CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            _logger.LogInformation("Fetching count of pending withdrawal requests.");

            try
            {
                // هنا نستدعي الدالة الجاهزة من الـ Repo
                var count = await _unitOfWork.WithdrawalRequests.GetTotalCountByStatusAsync(
                    WithdrawalStatus.Pending,
                    cancellationToken
                );

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get pending withdrawal requests count.");
                // يمكنك رمي استثناء مخصص هنا أو إرجاع 0 حسب منطق التطبيق
                throw new ServiceException("Failed to retrieve pending requests count", ex);
            }
        }


        // This is for API TO API
        public async Task ProcessCashbackBatchAsync(List<CashbackBatchItemDto> cashbackItems, CancellationToken cancellationToken = default)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var item in cashbackItems)
                {
                    // Note: It's crucial to ensure wallet exists or create it if not.
                    // The current CreateWalletAsync checks for existence.
                    await CreateWalletAsync(item.CustomerId); // يضمن وجود المحفظة للعميل

                    var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(item.CustomerId,track:true, cancellationToken)
                        ?? throw new NotFoundException($"المحفظة غير موجودة للعميل ID: {item.CustomerId}");

                    wallet.Balance += item.Amount;

                    // تسجيل حركة الإيداع
                    var transaction = new WalletTransaction
                    {
                        WalletId = wallet.Id,
                        Amount = item.Amount,
                        Type = TransactionType.Deposit, // نوع المعاملة إيداع
                        TransactionDate = DateTime.UtcNow,
                        Description = item.Description ?? "Cashback credit",
                        SourceId = item.OrderId, // لو فيه رقم طلب
                        SourceType = "CashbackBatch" // نوع المصدر عشان تعرف إنها جاية من الكاش باك
                    };
                    await _unitOfWork.WalletTransactions.AddAsync(transaction, cancellationToken);
                    _unitOfWork.Wallets.Update(wallet); // تحديث رصيد المحفظة
                }

                await _unitOfWork.SaveChangesAsync(); // حفظ كل التغييرات في دفعة واحدة
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Successfully processed cashback batch for {Count} customers.", cashbackItems.Count);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to process cashback batch.");
                throw new ServiceException("فشل في معالجة دفعة الكاش باك", ex);
            }
        }
    }
}
