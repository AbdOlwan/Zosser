using DAL_OnlineStore.Entities.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.People
{
    public class CustomerAddress
    {
        [Key]
        public int AddressID { get; set; }
        public int CustomerID { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }

        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string Country { get; set; } = null!;
        public bool IsDefault { get; set; }

        public Customer customer { get; set; } =null!;
        public IList<Order>? orders {  get; set; }
        
    }
}
