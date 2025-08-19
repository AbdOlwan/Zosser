using BLL.Exceptions;
using BLL.Services.Interfaces.Payments.Wallets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.Payments.Wallets;
using Shared.Interfaces;
using Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Payments.Wallets
{
    [Route("api/v1/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(
            IWalletService walletService,
            ICurrentUserService currentUserService,
            ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _currentUserService = currentUserService;
            _logger = logger;
        }




        /// <summary>
        /// Gets wallet details and transactions for a customer
        /// </summary>
        
        [HttpGet("{customerId:int}")]
        [Authorize(Policy = Policies.RequireCustomerRole)]
        [ProducesResponseType(typeof(ApiResponse<WalletResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWalletByCustomerId(
            [Range(1, int.MaxValue)] int customerId)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            try
            {
                //Authorization check
                if (!_currentUserService.IsInRole(Roles.Admin) &&
                    _currentUserService.GetCustomerId() != customerId)
                {
                    return ApiResponse.CreateUnauthorized(ValidationMessages.UnauthorizedAccess)
                                     .ToActionResult();
                }

                var wallet = await _walletService.GetWalletByCustomerIdAsync( customerId);

                return ApiResponse<WalletResponseDto>.CreateSuccess(wallet, ApiMessages.Success)
                                                     .ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Wallet not found for customer ID: {CustomerId}", customerId);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wallet for customer ID: {CustomerId}", customerId);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError)
                                  .ToActionResult();
            }
        }





        /// <summary>
        /// Creates a new withdrawal request
        /// </summary>
        [HttpPost("withdrawals")]
        [Authorize(Policy = Policies.RequireCustomerRole)]
        [ProducesResponseType(typeof(ApiResponse<WithdrawalRequestResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateWithdrawalRequest([FromBody] WithdrawalRequestCreateDto requestDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            if (!ModelState.IsValid)
                return ApiResponse.CreateBadRequest(ModelState).ToActionResult();

            try
            {
                var withdrawalRequest = await _walletService.CreateWithdrawalRequestAsync(requestDto);
                return ApiResponse<WithdrawalRequestResponseDto>.CreateCreated(
                    withdrawalRequest,
                    "تم تقديم طلب السحب بنجاح، جاري المراجعة"
                ).ToActionResult();
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("Business rule violation: {Message}", ex.Message);
                return ApiResponse.CreateBadRequest(ex.Message).ToActionResult();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized withdrawal attempt: {Message}", ex.Message);
                return ApiResponse.CreateUnauthorized(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating withdrawal request");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError)
                                  .ToActionResult();
            }
        }




        /// <summary>
        /// Processes a withdrawal request (Admin/Manager only)
        /// </summary>
        [HttpPut("withdrawals/{requestId:int}/process")]
        [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse<WithdrawalRequestResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProcessWithdrawalRequest( [Range(1, int.MaxValue)] int requestId,
                                                                 [FromBody] WithdrawalRequestUpdateDto updateDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            try
            {
                var processedRequest = await _walletService.ProcessWithdrawalRequestAsync(
                    requestId, updateDto);

                return ApiResponse<WithdrawalRequestResponseDto>.CreateSuccess(
                    processedRequest,
                    "تم تحديث حالة طلب السحب بنجاح"
                ).ToActionResult();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized withdrawal processing attempt");
                return ApiResponse.CreateUnauthorized(ex.Message).ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Withdrawal request not found: ID {RequestId}", requestId);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing withdrawal request ID: {RequestId}", requestId);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError)
                                  .ToActionResult();
            }
        }




        /// <summary>
        /// Gets withdrawal requests for a customer
        /// </summary>
        [HttpGet("{customerId:int}/withdrawals")]
        [Authorize(Policy = Policies.RequireCustomerRole)]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<WithdrawalRequestResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetWithdrawalRequestsForCustomer(
            [Range(1, int.MaxValue)] int customerId,
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            try
            {
                //Authorization check
                if (!_currentUserService.IsInRole(Roles.Admin) &&
                    _currentUserService.GetCustomerId() != customerId)
                {
                    return ApiResponse.CreateUnauthorized(ValidationMessages.UnauthorizedAccess)
                                     .ToActionResult();
                }

                var requests = await _walletService.GetWithdrawalRequestsForCustomerAsync(
                    customerId, pageNumber, pageSize);

                return ApiResponse<PagedResponse<WithdrawalRequestResponseDto>>.CreateSuccess(
                    requests,
                    ApiMessages.Success
                ).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving withdrawals for customer ID: {CustomerId}", customerId);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError)
                                  .ToActionResult();
            }
        }




        /// <summary>
        /// Gets withdrawal requests for admin review
        /// </summary>
        [HttpGet("withdrawals")]
        [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<WithdrawalRequestResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetWithdrawalRequestsForAdmin(
            [FromQuery] WithdrawalStatus status = WithdrawalStatus.Pending,
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            try
            {
                // Admin authorization
                if (!_currentUserService.IsInAnyRole(Roles.Admin))
                {
                    return ApiResponse.CreateUnauthorized(ValidationMessages.UnauthorizedAccess)
                                     .ToActionResult();
                }

                var requests = await _walletService.GetWithdrawalRequestsForAdminAsync(
                    status, pageNumber, pageSize);

                return ApiResponse<PagedResponse<WithdrawalRequestResponseDto>>.CreateSuccess(
                    requests,
                    ApiMessages.Success
                ).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving withdrawals with status: {Status}", status);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError)
                                  .ToActionResult();
            }
        }





        /// <summary>
        /// Gets the count of pending withdrawal requests for admin review.
        /// </summary>
        [HttpGet("withdrawals/pending-count")]
        [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPendingWithdrawalRequestsCount()
        {
            using var activity = ActivitySourceProvider.StartActivity();

            try
            {
                // Admin authorization
                if (!_currentUserService.IsInAnyRole(Roles.Admin))
                {
                    return ApiResponse.CreateUnauthorized(ValidationMessages.UnauthorizedAccess)
                                     .ToActionResult();
                }

                // ✅ هذا السطر سيعمل الآن بنجاح
                var count = await _walletService.GetPendingWithdrawalRequestsCountAsync();

                return ApiResponse<int>.CreateSuccess(
                    count,
                    ApiMessages.Success
                ).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending withdrawal requests count.");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError)
                                  .ToActionResult();
            }
        }




        /// <summary>
        /// Processes a batch of cashback additions for multiple customers (Admin/Internal System only)
        /// </summary>
        [HttpPost("process-cashback-batch")]
        [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessCashbackBatch(
            [FromBody] List<CashbackBatchItemDto> cashbackItems)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            if (!ModelState.IsValid)
                return ApiResponse.CreateBadRequest(ModelState).ToActionResult();

            // مهم جداً: تأكد إن الـ API دي محمية بشكل صارم
            // مفيش عميل عادي يقدر يكلمها. لازم تكون مخصصة لنظام الكاش باك فقط
            // ممكن عن طريق API Key، أو IP Whitelisting، أو دور (Role) خاص
            if (!_currentUserService.IsInRole(Roles.Admin) /* && add check for API Key or specific internal role */ )
            {
                return ApiResponse.CreateUnauthorized(ValidationMessages.UnauthorizedAccess)
                                 .ToActionResult();
            }

            if (cashbackItems == null || !cashbackItems.Any())
            {
                return ApiResponse.CreateBadRequest("قائمة الكاش باك لا يمكن أن تكون فارغة.").ToActionResult();
            }

            try
            {
                await _walletService.ProcessCashbackBatchAsync(cashbackItems);
                return ApiResponse.CreateSuccess(ApiMessages.Success, "تم معالجة دفعة الكاش باك بنجاح.").ToActionResult();
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error processing cashback batch.");
                return ApiResponse.CreateInternalServerError(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing cashback batch.");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }
    }
}
