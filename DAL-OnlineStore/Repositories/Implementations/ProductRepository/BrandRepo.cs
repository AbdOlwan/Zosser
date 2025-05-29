using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.ProductRepository
{
    public class BrandRepo : IBrandRepo
    {
        private readonly AppDbContext _context;

        public BrandRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>?> GetAllBrandsAsync()
        {
            return await _context.Brands
                            .Include(b=>b.BrandTranslations)
                            .AsNoTracking()
                            .ToListAsync();
        }
        public async Task<Brand?> getBrandById(int brandId, bool includeTranslations = false)
        {
            var query =_context.Brands.AsQueryable();
            if (includeTranslations)
            {
                query= query.Include(b=> b.BrandTranslations);
            }
            return await query.FirstOrDefaultAsync(b => b.Brand_ID == brandId);
        }

        public async Task<Brand> addNewBrand(Brand Brand)
        {
            await _context.Brands.AddAsync(Brand);
            await _context.SaveChangesAsync();
            return Brand;
        }
        public async Task<bool> deleteBrandById(int id)
        {
            var Brand = await _context.Brands.FirstOrDefaultAsync(d => d.Brand_ID == id);
            if (Brand == null)
            {
                return false;
            }

            _context.Brands.Remove(Brand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateBrand(Brand brand)
        {
            // تحميل البراند مع الترجمات لو محتاجهم لاحقًا
            var existingBrand = await _context.Brands
                .Include(b => b.BrandTranslations)
                .FirstOrDefaultAsync(b => b.Brand_ID == brand.Brand_ID);

            if (existingBrand != null)
                

             // تحديث الخصائص الأساسية للبراند (بدون ID)
             _context.Entry(existingBrand).CurrentValues.SetValues(brand);

            // ملاحظة: لو فيه تعديلات على BrandTranslations، يتم التعامل معها بشكل منفصل هنا

            await _context.SaveChangesAsync();
           
        }

        public async Task<bool> BrandExistsAsync(int id)
        {
            return await _context.Brands.AnyAsync(b => b.Brand_ID == id);
        }

    }
}
