using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductType = DAL_OnlineStore.Entities.Models.ProductModels.ProductType;

namespace DAL_OnlineStore.Repositories.Implementations.ProductRepository
{
    public class TypeRepo : ITypeRepo
    {
        private readonly AppDbContext _context;

        public TypeRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductType>?> getAllTypes()
        {
            return await _context.Types
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<ProductType?> getTypeById(int Id)
        {
            return await _context.Types.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.TypeId == Id);
        }

        public async Task<ProductType> addNewType(ProductType Type)
        {
            await _context.Types.AddAsync(Type);
            await _context.SaveChangesAsync();
            return Type;
        }
        public async Task<bool> deleteTypeById(int id)
        {
            var Type = await _context.Types.FirstOrDefaultAsync(d => d.TypeId == id);
            if (Type == null)
            {
                return false;
            }

            _context.Types.Remove(Type);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateTypeById(ProductType Type)
        {
            var result = await _context.Types.FirstOrDefaultAsync(d => d.TypeId == Type.TypeId);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(Type);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> TypeExistsAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return false;

                return await _context.Types
                    .AsNoTracking()
                    .AnyAsync(c => c.TypeId == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking type existence with ID: {id}", ex);
            }
        }


    }
}
