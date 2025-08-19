using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Shipments
{
    public class DeliveryAgentCreateDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(11, ErrorMessage = "Phone number cannot exceed 11 characters")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(250, ErrorMessage = "Email cannot exceed 250 characters")]
        public string? Email { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }

    public class DeliveryAgentUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        [StringLength(11, ErrorMessage = "Phone number cannot exceed 11 characters")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(250, ErrorMessage = "Email cannot exceed 250 characters")]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }
    }

    public class DeliveryAgentResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public int TotalCollections { get; set; } // Number of payment collections
    }

    public class DeliveryAgentCardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int ActiveCollections { get; set; } // Unverified collections
        public int TotalCollections { get; set; } // All collections
    }
}
