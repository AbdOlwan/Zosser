using DAL.Entities.Models.PaymentsModels;

namespace DAL.Repositories.Interfaces.Payments
{
    public interface IPaymentCollectionRepo
    {
        Task<PaymentCollection> CreatePaymentCollectionAsync(PaymentCollection paymentCollection, CancellationToken cancellationToken = default);
        Task UpdatePaymentCollectionAsync(PaymentCollection paymentCollection, CancellationToken cancellationToken = default);
        Task DeletePaymentCollectionAsync(int paymentCollectionId, CancellationToken cancellationToken = default);
        Task<PaymentCollection?> GetPaymentCollectionByIdAsync(int paymentCollectionId, bool track = false, CancellationToken cancellationToken = default);
        Task<PaymentCollection?> GetPaymentCollectionByPaymentIdAsync(int paymentId, bool track = false, CancellationToken cancellationToken = default); // Get by PaymentId
        Task<IEnumerable<PaymentCollection>> GetCollectionsByDeliveryAgentIdAsync(int deliveryAgentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentCollection>> GetCollectionsByVerificationStatusAsync(bool isVerified, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentCollection>> GetAllPaymentCollectionsAsync(CancellationToken cancellationToken = default);
        Task<bool> PaymentCollectionExistsAsync(int paymentCollectionId, CancellationToken cancellationToken = default);
    }
}
