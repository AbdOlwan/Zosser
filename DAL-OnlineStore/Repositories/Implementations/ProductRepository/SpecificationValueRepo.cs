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
    public class SpecificationValueRepo : ISpecificationValueRepo
    {
        private readonly AppDbContext _context;

        public SpecificationValueRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SpecificationValue>?> getAllSpecificationValues()
        {
            return await _context.SpecificationValues
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<SpecificationValue?> getSpecificationValueById(int Id)
        {
            return await _context.SpecificationValues.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.Value_id == Id);
        }

        public async Task<SpecificationValue> addNewSpecificationValue(SpecificationValue SpecificationValue)
        {
            await _context.SpecificationValues.AddAsync(SpecificationValue);
            await _context.SaveChangesAsync();
            return SpecificationValue;
        }
        public async Task<bool> deleteSpecificationValueById(int id)
        {
            var SpecificationValue = await _context.SpecificationValues.FirstOrDefaultAsync(d => d.Value_id == id);
            if (SpecificationValue == null)
            {
                return false;
            }

            _context.SpecificationValues.Remove(SpecificationValue);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateSpecificationValueById(SpecificationValue SpecificationValue)
        {
            var result = await _context.SpecificationValues.FirstOrDefaultAsync(d => d.Value_id == SpecificationValue.Value_id);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(SpecificationValue);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
