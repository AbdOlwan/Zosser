using DAL.Context;
using DAL.Entities.Models.FaqModels;
using DAL.Repositories.Interfaces.FAQs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace DAL.Repositories.Implementations.FAQs
{
    public class SiteFAQRepo : BaseFAQRepo<SiteFAQ>, ISiteFAQRepo
    {
        public SiteFAQRepo(
            ApplicationDbContext context,
            ILogger<SiteFAQRepo> logger,
            ICurrentUserService currentUserService)
            : base(context, logger, currentUserService)
        {
        }

        protected override DbSet<SiteFAQ> DbSet => _context.SiteFAQs;
    }
}
