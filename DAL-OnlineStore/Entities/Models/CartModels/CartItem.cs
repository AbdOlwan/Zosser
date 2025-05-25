using DAL_OnlineStore.Entities.Models.ProductModels;
using Microsoft.EntityFrameworkCore;


namespace DAL_OnlineStore.Entities.Models.CartModels
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }
        [Precision(10, 2)]
        public decimal Discount { get; set; }

        public Cart? Cart { get; set; }
        public Product? product { get; set; }
    }
}
