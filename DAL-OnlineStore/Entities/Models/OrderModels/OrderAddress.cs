using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.OrderModels
{
    public class OrderAddress
    {
        [Key]
        public int OrderAddressID { get; set; }

        // FK إلى Order — لا حاجة لحقل مكرر في Order نفسه
        [Required]
        public int OrderID { get; set; }
        public Order Order { get; set; } = null!;

        // ─── بيانات العنوان ────────────────────────────────────────
        [Required, StringLength(200)]
        public string AddressLine1 { get; set; } = null!;

        [StringLength(200)]
        public string? AddressLine2 { get; set; }

        [Required, StringLength(100)]
        public string City { get; set; } = null!;

        [Required, StringLength(100)]
        public string State { get; set; } = null!;

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [Required, StringLength(100)]
        public string Country { get; set; } = null!;

        // ─── حقول التدقيق (Audit) إن رغبت ────────────────────────────
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
    }


}
