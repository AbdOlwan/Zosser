using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Payments
{
    public enum PaymentStatus
    {
        Pending = 0, 
        Completed = 1, 
        Failed = 2,
        Refunded = 3,
        Cancelled = 4, 
        CollectedFromCustomer = 5 
    }
    public enum PaymentMethod
    {
        ZosserWallet,    
        CashOnDelivery    
    }
    public class PaymentCreateDTO
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
        [Required]
        public PaymentMethod Method { get; set; } = PaymentMethod.CashOnDelivery;
        public string? TransactionId { get; set; }
    }
    public class PaymentUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        public PaymentStatus? Status { get; set; }
        public string? TransactionId { get; set; }
    }
    public class PaymentResponseDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
    }
    public class PaymentCollectionCreateDTO
    {
        [Required]
        public int PaymentId { get; set; }
        [Required]
        public int DeliveryAgentId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
    public class PaymentCollectionResponseDTO
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal CollectedAmount { get; set; }
        public DateTime CollectedAt { get; set; }
        public int DeliveryAgentId { get; set; }
        public string DeliveryAgentName { get; set; } = string.Empty;
        public string? DeliveryAgentPhone { get; set; }
        public string? DeliveryAgentEmail { get; set; }
        public string? VerifiedByUserId { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
