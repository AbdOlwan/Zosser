using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.CartModels
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserID { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public CartStatus CartStatus { get; set; } = CartStatus.Active;
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public IList<CartItem>? CartItems { get; set; }  

    }

    public enum CartStatus
    {
        Active = 0,
        Ordered = 1,
        Saved = 2
    }
}
