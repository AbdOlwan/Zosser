using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models;
using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using Microsoft.EntityFrameworkCore;


namespace DAL_OnlineStore.Repositories.Implementations.ProductRepository
{
    public class ProductImageRepo : IProductImageRepo
    {
        private readonly AppDbContext _context;

        public ProductImageRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductImage>?> getAllProductImages()
        {
            return await _context.ProductImages
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<ProductImage?> getProductImageById(int Id)
        {
            return await _context.ProductImages.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.ImageID == Id);
        }

        public async Task<ProductImage> addNewProductImage(ProductImage productImage)
        {
            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync();
            return productImage;
        }
        public async Task<bool> deleteProductImageById(int id)
        {
            var ProductImage = await _context.ProductImages.FirstOrDefaultAsync(d => d.ImageID == id);
            if (ProductImage == null)
            {
                return false;
            }

            _context.ProductImages.Remove(ProductImage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateProductImageById(ProductImage productImage)
        {
            var result = await _context.ProductImages.FirstOrDefaultAsync(d => d.ImageID == productImage.ImageID);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(productImage);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> countProductImages(int ProductId)
        {
            return await _context.ProductImages
                .Where(d => d.ProductID == ProductId)
                .CountAsync();
        }
    }
}
