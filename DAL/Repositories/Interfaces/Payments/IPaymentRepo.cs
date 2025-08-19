using DAL.Entities.Models.PaymentsModels;

namespace DAL.Repositories.Interfaces.Payments
{
    public interface IPaymentRepo
    {
        Task<Payment> CreatePaymentAsync(Payment payment, CancellationToken cancellationToken = default);
        Task UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken = default);
        Task DeletePaymentAsync(int paymentId, CancellationToken cancellationToken = default);
        Task<Payment?> GetPaymentByIdAsync(int paymentId, bool track = false, CancellationToken cancellationToken = default);
        Task<Payment?> GetPaymentByOrderIdAsync(int orderId, bool track = false, CancellationToken cancellationToken = default); // Changed to single payment per order
        Task<IEnumerable<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken = default);
        Task<bool> PaymentExistsAsync(int paymentId, CancellationToken cancellationToken = default);
        Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId, bool track = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<Payment>> GetPaymentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    }
}
