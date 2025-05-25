


using System.ComponentModel.DataAnnotations;

namespace DAL_OnlineStore.Entities.Models.ProductModels
{
    public class ProductImage
    {
        [Key]
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string ImageURL { get; set; } = null!;
        public bool IsPrimary { get; set; }

        public Product Product { get; set; } = null!;

    }
}
