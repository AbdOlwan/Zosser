using DAL_OnlineStore.Entities.Models;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
{
    public interface IProductImageRepo
    {
        Task<List<ProductImage>?> getAllProductImages();
        Task<ProductImage> addNewProductImage(ProductImage productImage);

        Task<int> countProductImages(int ProductId);

        Task<ProductImage?> getProductImageById(int id);

        Task<bool> deleteProductImageById(int id);

        Task<bool> updateProductImageById(ProductImage productImage);
    }
}
