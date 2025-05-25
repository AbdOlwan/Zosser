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
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>?> getAllCategorys()
        {
            return await _context.Categories
                                    .AsNoTracking()
                                    .Include(c => c.Translations
                                    .Where(t=>t.Culture == _context.CurrentCulture))
                                    .ToListAsync();
        }

        public async Task<Category?> getCategoryById(int Id)
        {
            return await _context.Categories.AsNoTracking()
                            .Include(c => c.Translations
                            .Where(t=>t.Culture == _context.CurrentCulture))
                            .FirstOrDefaultAsync(a => a.CategoryID == Id);
        }

        public async Task<Category> addNewCategory(Category Category)
        {
            await _context.Categories.AddAsync(Category);
            await _context.SaveChangesAsync();
            return Category;
        }
        public async Task<bool> deleteCategoryById(int id)
        {
            var Category = await _context.Categories.FirstOrDefaultAsync(d => d.CategoryID == id);
            if (Category == null)
            {
                return false;
            }

            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateCategoryById(Category Category)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(d => d.CategoryID == Category.CategoryID);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(Category);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
