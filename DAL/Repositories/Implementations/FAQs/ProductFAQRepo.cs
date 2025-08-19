using DAL.Context;
using DAL.Entities.Models.FaqModels;
using DAL.Repositories.Interfaces.FAQs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace DAL.Repositories.Implementations.FAQs
{
    public class ProductFAQRepo : BaseFAQRepo<ProductFAQ>, IProductFAQRepo
    {
        public ProductFAQRepo(
            ApplicationDbContext context,
            ILogger<ProductFAQRepo> logger,
            ICurrentUserService currentUserService)
            : base(context, logger, currentUserService)
        {
        }

        protected override DbSet<ProductFAQ> DbSet => _context.ProductFAQs;

        public async Task<IEnumerable<ProductFAQ>> GetByProductIdAsync(int productId)
        {
            try
            {
                return await DbSet
                    .Where(f => f.ProductId == productId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting ProductFAQs by Product ID: {productId}");
                throw new ApplicationException($"Failed to get ProductFAQs for Product ID: {productId}", ex);
            }
        }
    }
}
