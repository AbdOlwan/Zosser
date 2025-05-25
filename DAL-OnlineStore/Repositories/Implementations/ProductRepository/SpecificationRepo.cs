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
    public class SpecificationRepo : ISpecificationRepo
    {
        private readonly AppDbContext _context;

        public SpecificationRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Specification>?> getAllSpecifications()
        {
            return await _context.Specifications
                                    .AsNoTracking()
                                    .Include(s=>s.specificationTranslations
                                    .Where(t=>t.Culture == _context.CurrentCulture))
                                    .ToListAsync();
        }

        public async Task<Specification?> getSpecificationById(int Id)
        {
            return await _context.Specifications.AsNoTracking()
                                                .Include(s=>s.specificationTranslations
                                                .Where(t=>t.Culture== _context.CurrentCulture))
                                                .FirstOrDefaultAsync(a => a.SpecificationId == Id);
        }

        public async Task<Specification> addNewSpecification(Specification Specification)
        {
            await _context.Specifications.AddAsync(Specification);
            await _context.SaveChangesAsync();
            return Specification;
        }
        public async Task<bool> deleteSpecificationById(int id)
        {
            var Specification = await _context.Specifications.FirstOrDefaultAsync(d => d.SpecificationId == id);
            if (Specification == null)
            {
                return false;
            }

            _context.Specifications.Remove(Specification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateSpecificationById(Specification Specification)
        {
            var result = await _context.Specifications.FirstOrDefaultAsync(d => d.SpecificationId == Specification.SpecificationId);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(Specification);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
