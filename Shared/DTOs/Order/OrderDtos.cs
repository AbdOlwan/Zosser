using Shared.DTOs.CustomerDtos;
using Shared.DTOs.Payments;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Order
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Preparing,
        Shipped,
        Delivered,
        Cancelled
    }

    public record OrderForUpdateDto
    {
        [Required]
        public OrderStatus Status { get; set; }

        [Required, MaxLength(150)]
        public string ContactName { get; set; } = null!;

        [Required, MaxLength(11)]
        public string PhoneNumber { get; set; } = null!;

        [Required, MaxLength(200)]
        public string AddressLine1 { get; set; } = null!;

        [MaxLength(200)]
        public string? AddressLine2 { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; } = null!;

        [Required, MaxLength(100)]
        public string State { get; set; } = null!;
    }

    public record OrderForCreationDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required, MaxLength(255)]
        public string ProductName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string ProductColor { get; set; } = null!;

        [Required, MaxLength(50)]
        public string ProductSize { get; set; } = null!;

        [Required]
        public decimal ShippingCost { get; set; }

        [Required]
        public decimal CodFee { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public decimal TotalCashback { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required, MaxLength(150)]
        public string ContactName { get; set; } = null!;

        [Required, MaxLength(11)]
        public string PhoneNumber { get; set; } = null!;

        [Required, MaxLength(200)]
        public string AddressLine1 { get; set; } = null!;

        [MaxLength(200)]
        public string? AddressLine2 { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; } = null!;

        [Required, MaxLength(100)]
        public string State { get; set; } = null!;
    }

    public record OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductColor { get; set; } = null!;
        public string ProductSize { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal CodFee { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalCashback { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; } 
        public string ContactName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
    }

    public record OrderDetailsDto : OrderDto
    {
        // Add customer details to the order details
        public CustomerSummaryDto Customer { get; set; } = null!;
    }
}