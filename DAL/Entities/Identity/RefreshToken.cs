using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Identity
{
    public class RefreshToken : IAuditableEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAt { get; set; }

        public string? RevokedBy { get; set; }

        public string? ReplacedByToken { get; set; }

        public string? ReasonRevoked { get; set; }

        // Navigation Properties
        public virtual ApplicationUser User { get; set; } = null!;

        // IAuditableEntity Properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public DateTime LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;

        // Helper Properties
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsValidForUse => IsActive && !IsRevoked && !IsExpired;
    }
}
