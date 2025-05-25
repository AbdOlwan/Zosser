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

        public async Task<List<Brand>?> getAllBrands()
        {
            return await _context.Brands
                                    .AsNoTracking()
                                    .Include(b=>b.BrandTranslations
                                    .Where(t=>t.Culture == _context.CurrentCulture))
                                    .ToListAsync();
        }

        public async Task<Brand?> getBrandById(int BrandId)
        {
            return await _context.Brands.AsNoTracking()
                                           .Include(b=>b.BrandTranslations
                                           .Where(t=>t.Culture == _context.CurrentCulture))
                                            .FirstOrDefaultAsync(a => a.Brand_ID == BrandId);
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

        public async Task<bool> updateBrandById(Brand Brand)
        {
            var result = await _context.Brands.FirstOrDefaultAsync(d => d.Brand_ID == Brand.Brand_ID);
            if (result != null)
            {
                result.Brand_ID = Brand.Brand_ID;
                _context.Entry(result).CurrentValues.SetValues(Brand);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
