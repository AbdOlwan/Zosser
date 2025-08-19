using BLL.Services.Interfaces.FAQs;
using DAL.Entities.Models.FaqModels;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Interfaces.FAQs;
using Mapster;
using Microsoft.Extensions.Logging;
using Shared.DTOs.FAQs;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations.FAQs
{
    public class SiteFAQService : BaseFAQService<SiteFAQ, SiteFAQReadDto, SiteFAQCreateDto, SiteFAQUpdateDto>, ISiteFAQService
    {
        private readonly ISiteFAQRepo _siteFAQRepo;

        public SiteFAQService(
            IUnitOfWork unitOfWork,
            ILogger<SiteFAQService> logger)
            : base(unitOfWork, logger)
        {
            _siteFAQRepo = unitOfWork.SiteFAQs;
        }

        protected override IBaseFAQRepo<SiteFAQ> GetRepo() => _siteFAQRepo;

        public async Task<IEnumerable<BaseFAQSimpleDto>> GetSiteFAQsAsync()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var faqs = await _siteFAQRepo.GetAllAsync();
                return faqs.Adapt<IEnumerable<BaseFAQSimpleDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting site FAQs");
                throw;
            }
        }
    }
}
