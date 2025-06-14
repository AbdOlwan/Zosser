﻿
using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.OrderModels
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public Order Order { get; set; } = null!;

        [Required]
        public int ProductID { get; set; }
        public Product? Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
    }
}