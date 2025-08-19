using Shared.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces.Payments
{    /// <summary>
     /// Service for handling payment and payment collection logic.
     /// </summary>
    public interface IPaymentService
    {
        #region Payment Operations

        /// <summary>
        /// Creates a new payment for an order, ensuring it matches the order total.
        /// </summary>
        /// <param name="createDto">The DTO containing payment creation data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A DTO of the newly created payment.</returns>
        Task<PaymentResponseDTO> CreatePaymentAsync(PaymentCreateDTO createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a payment by its unique ID.
    /// </summary>
    Task<PaymentResponseDTO> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the payment associated with a specific order ID.
    /// </summary>
    Task<PaymentResponseDTO> GetPaymentByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of all payments.
    /// </summary>
    Task<IEnumerable<PaymentResponseDTO>> GetAllPaymentsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all payments made by a specific customer.
    /// </summary>
    Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByCustomerAsync(int customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status or transaction ID of an existing payment.
    /// </summary>
    Task UpdatePaymentAsync(PaymentUpdateDTO updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a payment if it's in a state that allows deletion (e.g., Pending).
    /// </summary>
    Task DeletePaymentAsync(int paymentId, CancellationToken cancellationToken = default);

    #endregion

    #region Payment Collection Operations

    /// <summary>
    /// Records the collection of a cash-on-delivery payment by a delivery agent.
    /// </summary>
    /// <param name="createDto">The DTO containing collection data.</param>
    /// <returns>A DTO of the newly created payment collection record.</returns>
    Task<PaymentCollectionResponseDTO> CreatePaymentCollectionAsync(PaymentCollectionCreateDTO createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific payment collection record by its ID.
    /// </summary>
    Task<PaymentCollectionResponseDTO> GetPaymentCollectionByIdAsync(int collectionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all payment collections made by a specific delivery agent.
    /// </summary>
    Task<IEnumerable<PaymentCollectionResponseDTO>> GetCollectionsByDeliveryAgentAsync(int deliveryAgentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves payment collections based on their verification status.
    /// </summary>
    Task<IEnumerable<PaymentCollectionResponseDTO>> GetCollectionsByVerificationStatusAsync(bool isVerified, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a payment collection record as verified by an authorized user.
    /// </summary>
    /// <param name="collectionId">The ID of the collection to verify.</param>
    /// <param name="verifiedByUserId">The ID of the user verifying the collection.</param>
    Task VerifyPaymentCollectionAsync(int collectionId, string verifiedByUserId, CancellationToken cancellationToken = default);

    #endregion
}
}
