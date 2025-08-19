using BLL.Exceptions;
using BLL.Services.Interfaces.Payments;
using DAL.Entities.Models.PaymentsModels;
using DAL.Repositories.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using Shared.DTOs.Payments;

namespace BLL.Services.Implementations.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IUnitOfWork unitOfWork, ILogger<PaymentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #region Payment Methods

        public async Task<PaymentResponseDTO> CreatePaymentAsync(PaymentCreateDTO createDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to create payment for Order ID: {OrderId}", createDto.OrderId);

            var order = await _unitOfWork.Orders.GetByIdAsync(createDto.OrderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {createDto.OrderId} not found.");
            }

            if (await _unitOfWork.Payments.GetPaymentByOrderIdAsync(createDto.OrderId, track: false, cancellationToken) != null)
            {
                throw new BusinessException($"A payment already exists for Order ID {createDto.OrderId}.");
            }

            if (createDto.Amount != order.TotalAmount)
            {
                throw new BusinessException($"Payment amount {createDto.Amount:C} does not match order total amount {order.TotalAmount:C}.");
            }


            try
            {
                var paymentEntity = createDto.Adapt<Payment>();
                var createdPayment = await _unitOfWork.Payments.CreatePaymentAsync(paymentEntity, cancellationToken);


                _logger.LogInformation("Successfully created payment ID {PaymentId} for Order ID {OrderId}", createdPayment.Id, createDto.OrderId);
                return createdPayment.Adapt<PaymentResponseDTO>();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Failed to create payment for Order ID: {OrderId}", createDto.OrderId);
                throw new ServiceException("An error occurred while creating the payment.", ex);
            }
        }

        public async Task<PaymentResponseDTO> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default)
        {
            var payment = await _unitOfWork.Payments.GetPaymentByIdAsync(paymentId, track: false, cancellationToken);
            if (payment == null)
            {
                throw new NotFoundException($"Payment with ID {paymentId} not found.");
            }
            return payment.Adapt<PaymentResponseDTO>();
        }

        public async Task<PaymentResponseDTO> GetPaymentByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
        {
            var payment = await _unitOfWork.Payments.GetPaymentByOrderIdAsync(orderId, track: false, cancellationToken);
            if (payment == null)
            {
                throw new NotFoundException($"No payment found for Order ID {orderId}.");
            }
            return payment.Adapt<PaymentResponseDTO>();
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetAllPaymentsAsync(CancellationToken cancellationToken = default)
        {
            var payments = await _unitOfWork.Payments.GetAllPaymentsAsync(cancellationToken);
            return payments.Adapt<IEnumerable<PaymentResponseDTO>>();
        }

        public async Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            if (await _unitOfWork.Customers.GetCustomerById(customerId) == null)
            {
                throw new NotFoundException($"Customer with ID {customerId} not found.");
            }

            var payments = await _unitOfWork.Payments.GetPaymentsByCustomerIdAsync(customerId, cancellationToken);
            return payments.Adapt<IEnumerable<PaymentResponseDTO>>();
        }

        public async Task UpdatePaymentAsync(PaymentUpdateDTO updateDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to update payment ID: {PaymentId}", updateDto.Id);

            var payment = await _unitOfWork.Payments.GetPaymentByIdAsync(updateDto.Id, track: true, cancellationToken);
            if (payment == null)
            {
                throw new NotFoundException($"Payment with ID {updateDto.Id} not found.");
            }

            payment.Status = updateDto.Status ?? payment.Status;
            payment.TransactionId = updateDto.TransactionId ?? payment.TransactionId;

            try
            {
                await _unitOfWork.Payments.UpdatePaymentAsync(payment, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update payment ID: {PaymentId}", updateDto.Id);
                throw new ServiceException("An error occurred while updating the payment.", ex);
            }
        }

        public async Task DeletePaymentAsync(int paymentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to delete payment ID: {PaymentId}", paymentId);

            var payment = await _unitOfWork.Payments.GetPaymentByIdAsync(paymentId, track: false, cancellationToken);
            if (payment == null)
            {
                _logger.LogWarning("Attempted to delete a non-existent payment with ID: {PaymentId}", paymentId);
                return; // Idempotent
            }

            if (payment.Status != PaymentStatus.Pending && payment.Status != PaymentStatus.Failed && payment.Status != PaymentStatus.Cancelled)
            {
                throw new BusinessException($"Cannot delete a payment with status '{payment.Status}'.");
            }

            try
            {
                await _unitOfWork.Payments.DeletePaymentAsync(paymentId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment ID: {PaymentId}", paymentId);
                throw new ServiceException($"Failed to delete payment {paymentId}.", ex);
            }
        }

        #endregion

        #region Payment Collection Methods

        public async Task<PaymentCollectionResponseDTO> CreatePaymentCollectionAsync(PaymentCollectionCreateDTO createDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to create payment collection for Payment ID: {PaymentId}", createDto.PaymentId);

            var payment = await _unitOfWork.Payments.GetPaymentByIdAsync(createDto.PaymentId, track: true, cancellationToken);
            if (payment == null)
            {
                throw new NotFoundException($"Payment with ID {createDto.PaymentId} not found.");
            }

            if (payment.Method != PaymentMethod.CashOnDelivery)
            {
                throw new BusinessException("Payment collections are only for 'Cash on Delivery' payments.");
            }

            if (createDto.Amount != payment.Amount)
            {
                throw new BusinessException($"Collected amount {createDto.Amount:C} does not match payment amount {payment.Amount:C}.");
            }

            if (await _unitOfWork.PaymentCollections.GetPaymentCollectionByPaymentIdAsync(createDto.PaymentId, false, cancellationToken) != null)
            {
                throw new BusinessException($"A collection record already exists for Payment ID {createDto.PaymentId}.");
            }

            if (await _unitOfWork.DeliveryAgents.GetDeliveryAgentByIdAsync(createDto.DeliveryAgentId) == null) // Assuming GetByIdAsync exists
            {
                throw new NotFoundException($"Delivery Agent with ID {createDto.DeliveryAgentId} not found.");
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var collectionEntity = createDto.Adapt<PaymentCollection>();
                var createdCollection = await _unitOfWork.PaymentCollections.CreatePaymentCollectionAsync(collectionEntity, cancellationToken);

                payment.Status = PaymentStatus.CollectedFromCustomer;
                await _unitOfWork.Payments.UpdatePaymentAsync(payment, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Successfully created payment collection for Payment ID: {PaymentId}", createDto.PaymentId);

                var resultDto = await GetPaymentCollectionByIdAsync(createdCollection.Id, cancellationToken);
                return resultDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create payment collection for Payment ID: {PaymentId}", createDto.PaymentId);
                throw new ServiceException("An error occurred while creating the payment collection.", ex);
            }
        }

        public async Task<PaymentCollectionResponseDTO> GetPaymentCollectionByIdAsync(int collectionId, CancellationToken cancellationToken = default)
        {
            var collection = await _unitOfWork.PaymentCollections.GetPaymentCollectionByIdAsync(collectionId, track: false, cancellationToken);
            if (collection == null)
            {
                throw new NotFoundException($"Payment Collection with ID {collectionId} not found.");
            }
            return collection.Adapt<PaymentCollectionResponseDTO>();
        }

        public async Task<IEnumerable<PaymentCollectionResponseDTO>> GetCollectionsByDeliveryAgentAsync(int deliveryAgentId, CancellationToken cancellationToken = default)
        {
            var collections = await _unitOfWork.PaymentCollections.GetCollectionsByDeliveryAgentIdAsync(deliveryAgentId, cancellationToken);
            return collections.Adapt<IEnumerable<PaymentCollectionResponseDTO>>();
        }

        public async Task<IEnumerable<PaymentCollectionResponseDTO>> GetCollectionsByVerificationStatusAsync(bool isVerified, CancellationToken cancellationToken = default)
        {
            var collections = await _unitOfWork.PaymentCollections.GetCollectionsByVerificationStatusAsync(isVerified, cancellationToken);
            return collections.Adapt<IEnumerable<PaymentCollectionResponseDTO>>();
        }

        public async Task VerifyPaymentCollectionAsync(int collectionId, string verifiedByUserId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to verify payment collection ID: {CollectionId} by User: {VerifiedByUserId}", collectionId, verifiedByUserId);

            var collection = await _unitOfWork.PaymentCollections.GetPaymentCollectionByIdAsync(collectionId, track: true, cancellationToken);
            if (collection == null)
            {
                throw new NotFoundException($"Payment Collection with ID {collectionId} not found.");
            }

            if (collection.VerifiedAt.HasValue)
            {
                throw new BusinessException($"Payment Collection {collectionId} was already verified.");
            }

            collection.VerifiedAt = DateTime.UtcNow;
            collection.VerifiedByUserId = verifiedByUserId;

            try
            {
                await _unitOfWork.PaymentCollections.UpdatePaymentCollectionAsync(collection, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify payment collection ID: {CollectionId}", collectionId);
                throw new ServiceException("An error occurred while verifying the collection.", ex);
            }
        }

        #endregion
    }
}
