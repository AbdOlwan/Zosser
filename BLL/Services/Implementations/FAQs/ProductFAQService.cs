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
    public class ProductFAQService : BaseFAQService<ProductFAQ, ProductFAQReadDto, ProductFAQCreateDto, ProductFAQUpdateDto>, IProductFAQService
    {
        private readonly IProductFAQRepo _productFAQRepo;
        private readonly ISiteFAQRepo _siteFAQRepo;

        public ProductFAQService(
            IUnitOfWork unitOfWork,
            ILogger<ProductFAQService> logger)
            : base(unitOfWork, logger)
        {
            _productFAQRepo = unitOfWork.ProductFAQs;
            _siteFAQRepo = unitOfWork.SiteFAQs;
        }

        protected override IBaseFAQRepo<ProductFAQ> GetRepo() => _productFAQRepo;

        public async Task<IEnumerable<ProductFAQSimpleDto>> GetByProductIdAsync(int productId)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var faqs = await _productFAQRepo.GetByProductIdAsync(productId);
                return faqs.Adapt<IEnumerable<ProductFAQSimpleDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting FAQs for product ID: {ProductId}", productId);
                throw;
            }
        }

        public async Task<FAQCollectionDto> GetGroupedFAQsAsync()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var siteFAQs = await _siteFAQRepo.GetAllAsync();
                var productFAQs = await _productFAQRepo.GetAllAsync();

                return new FAQCollectionDto
                {
                    SiteFAQs = siteFAQs.Adapt<IEnumerable<BaseFAQSimpleDto>>(),
                    ProductFAQs = productFAQs.Adapt<IEnumerable<BaseFAQSimpleDto>>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grouped FAQs");
                throw;
            }
        }
    }
}
