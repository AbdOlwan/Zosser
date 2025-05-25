using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.ProductRepository
{
    public class ProductSpecificationRepo : IProductSpecificationRepo
    {
        private readonly AppDbContext _context;

        public ProductSpecificationRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSpecification>?> getAllProductSpecifications()
        {
            return await _context.ProductSpecifications
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<ProductSpecification> addNewProductSpecification(ProductSpecification ProductSpecification)
        {
            await _context.ProductSpecifications.AddAsync(ProductSpecification);
            await _context.SaveChangesAsync();
            return ProductSpecification;
        }

        //public async Task<ProductSpecification?> getProductSpecificationById(int Id)
        //{
        //    return await _context.ProductSpecifications.AsNoTracking()
        //                                    .FirstOrDefaultAsync(a => a.ProductSpecificationID == Id);
        //}


        //public async Task<bool> deleteProductSpecificationById(int id)
        //{
        //    var ProductSpecification = await _context.ProductSpecifications.FirstOrDefaultAsync(d => d.pro == id);
        //    if (ProductSpecification == null)
        //    {
        //        return false;
        //    }

        //    _context.Categories.Remove(ProductSpecification);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> updateProductSpecificationById(ProductSpecification ProductSpecification)
        //{
        //    var result = await _context.ProductSpecifications.FirstOrDefaultAsync(d => d.ProductSpecificationID == ProductSpecification.ProductSpecificationID);
        //    if (result != null)
        //    {
        //        _context.Entry(result).CurrentValues.SetValues(ProductSpecification);

        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}
    }
}
