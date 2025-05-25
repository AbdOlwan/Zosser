using DAL_OnlineStore.Entities.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public string UserID { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public CartStatus CartStatus { get; set; } = CartStatus.Active;
        public decimal TotalPrice { get; set; }
    }
}
