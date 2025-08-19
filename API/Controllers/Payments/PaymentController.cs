using BLL.Exceptions;
using BLL.Services.Interfaces.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.Payments;
using Shared.Interfaces;
using Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Payments
{
    [Route("api/v1/payments")]
    [ApiController]
   // [Authorize] // Require authentication for all payment-related actions
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ICurrentUserService currentUserService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        #region Payment Endpoints


        [HttpGet]
        [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPayments()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                return ApiResponse<IEnumerable<PaymentResponseDTO>>.CreateSuccess(payments, ApiMessages.Success).ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No payments found: {Message}", ex.Message);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payments.");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// Creates a new payment record for an order.
        /// </summary>
        [HttpPost]
       // [Authorize(Policy = Policies.RequireAdminRole)] // Only admins can create payments directly
        [ProducesResponseType(typeof(ApiResponse<PaymentResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDTO createDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            if (!ModelState.IsValid)
                return ApiResponse.CreateBadRequest(ModelState).ToActionResult();

            try
            {
                var createdPayment = await _paymentService.CreatePaymentAsync(createDto);
                return ApiResponse<PaymentResponseDTO>.CreateCreated(createdPayment, ApiMessages.Created).ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Reference not found during payment creation: {Message}", ex.Message);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation during payment creation: {Message}", ex.Message);
                return ApiResponse.CreateConflict(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a payment.");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// Retrieves a specific payment by its ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<PaymentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentById([Range(1, int.MaxValue)] int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                return ApiResponse<PaymentResponseDTO>.CreateSuccess(payment, ApiMessages.Success).ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Payment with ID {PaymentId} not found.", id);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment with ID {PaymentId}", id);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// Retrieves all payments for a specific customer.
        /// </summary>
        [HttpGet("customer/{customerId:int}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.Customer}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentsByCustomer([Range(1, int.MaxValue)] int customerId)
        {
            using var activity = ActivitySourceProvider.StartActivity();

            // Security check: A customer can only view their own payments.
            if (_currentUserService.IsInRole(Roles.Customer) && _currentUserService.GetCustomerId() != customerId)
            {
                return ApiResponse.CreateForbidden("You are not authorized to view this customer's payments.").ToActionResult();
            }

            try
            {
                var payments = await _paymentService.GetPaymentsByCustomerAsync(customerId);
                return ApiResponse<IEnumerable<PaymentResponseDTO>>.CreateSuccess(payments, ApiMessages.Success).ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Attempted to get payments for a non-existent customer with ID {CustomerId}.", customerId);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payments for customer with ID {CustomerId}", customerId);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }



        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePayment([Range(1, int.MaxValue)] int id, [FromBody] PaymentUpdateDTO updateDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            if (!ModelState.IsValid)
                return ApiResponse.CreateBadRequest(ModelState).ToActionResult();
            try
            {
                await _paymentService.UpdatePaymentAsync( updateDto);
                return ApiResponse.CreateSuccess("Payment updated successfully.").ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Payment with ID {PaymentId} not found for update.", id);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation during payment update: {Message}", ex.Message);
                return ApiResponse.CreateBadRequest(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment with ID {PaymentId}.", id);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        #endregion

        #region Payment Collection Endpoints

        /// <summary>
        /// Records that a cash-on-delivery payment was collected by a delivery agent.
        /// </summary>
        [HttpPost("collections")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")] // Or a specific "DeliveryAgent" role
        [ProducesResponseType(typeof(ApiResponse<PaymentCollectionResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreatePaymentCollection([FromBody] PaymentCollectionCreateDTO createDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            if (!ModelState.IsValid)
                return ApiResponse.CreateBadRequest(ModelState).ToActionResult();

            try
            {
                var createdCollection = await _paymentService.CreatePaymentCollectionAsync(createDto);
                return ApiResponse<PaymentCollectionResponseDTO>.CreateCreated(createdCollection, "Payment collected successfully.").ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Reference not found during payment collection: {Message}", ex.Message);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation during payment collection: {Message}", ex.Message);
                return ApiResponse.CreateBadRequest(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a payment collection.");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// Verifies a payment collection record.
        /// </summary>
        [HttpPost("collections/{collectionId:int}/verify")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyPaymentCollection([Range(1, int.MaxValue)] int collectionId)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            var userId = _currentUserService.GetCurrentUserId();

            try
            {
                await _paymentService.VerifyPaymentCollectionAsync(collectionId, userId);
                return ApiResponse.CreateSuccess("Payment collection verified successfully.").ToActionResult();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Collection ID {CollectionId} not found for verification.", collectionId);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation during verification of collection ID {CollectionId}.", collectionId);
                return ApiResponse.CreateBadRequest(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying collection ID {CollectionId}.", collectionId);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// Gets payment collections by their verification status.
        /// </summary>
        [HttpGet("collections/status")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentCollectionResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCollectionsByVerificationStatus([FromQuery] bool isVerified = false)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var collections = await _paymentService.GetCollectionsByVerificationStatusAsync(isVerified);
                return ApiResponse<IEnumerable<PaymentCollectionResponseDTO>>.CreateSuccess(collections, ApiMessages.Success).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving collections by verification status (IsVerified: {IsVerified}).", isVerified);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        #endregion
    }
}
