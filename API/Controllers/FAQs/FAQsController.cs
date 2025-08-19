//using BLL.Services.Interfaces.FAQs;
//using Microsoft.AspNetCore.Mvc;
//using Shared.DTOs;
//using Shared.DTOs.FAQs;
//using Shared.Utils;

//namespace API.Controllers.FAQs
//{
//    [Route("api/v1/FAQs")]
//    [ApiController]
//    public class FAQsController : ControllerBase
//    {
//        private readonly ISiteFAQService _siteFAQService;
//        private readonly IProductFAQService _productFAQService;
//        private readonly ILogger<FAQsController> _logger;

//        public FAQsController(
//            ISiteFAQService siteFAQService,
//            IProductFAQService productFAQService,
//            ILogger<FAQsController> logger)
//        {
//            _siteFAQService = siteFAQService;
//            _productFAQService = productFAQService;
//            _logger = logger;
//        }

//        // ===== Site FAQ Endpoints =====

//        [HttpGet("site")]
//        public async Task<ApiResponse<IEnumerable<SiteFAQReadDto>>> GetAllSiteFAQs()
//        {
//            using var activity = ActivitySourceProvider.StartActivity();
//            try
//            {
//                var faqs = await _siteFAQService.GetAllAsync();
//                return ApiResponse<IEnumerable<SiteFAQReadDto>>.CreateSuccess(faqs);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving site FAQs");
//                return ApiResponse<IEnumerable<SiteFAQReadDto>>.CreateInternalServerError("Internal server error");
//            }
//        }

//        [HttpGet("site/{id}")]
//        public async Task<ApiResponse<SiteFAQReadDto>> GetSiteFAQById(int id)
//        {
//            using var activity = ActivitySourceProvider.StartActivity();
//            try
//            {
//                var faq = await _siteFAQService.GetByIdAsync(id);
//                return ApiResponse<SiteFAQReadDto>.CreateSuccess(faq);
//            }
//            catch (KeyNotFoundException ex)
//            {
//                _logger.LogWarning(ex, "Site FAQ not found: {Id}", id);
//                return ApiResponse<SiteFAQReadDto>.CreateNotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving site FAQ ID: {Id}", id);
//                return ApiResponse<SiteFAQReadDto>.CreateInternalServerError("Internal server error");
//            }
//        }

//        [HttpPost("site")]
//        public async Task<ApiResponse<SiteFAQReadDto>> CreateSiteFAQ([FromBody] SiteFAQCreateDto dto)
//        {
//            using var activity = ActivitySourceProvider.StartActivity();
//            try
//            {
//                var newFaq = await _siteFAQService.AddAsync(dto);
//                return ApiResponse<SiteFAQReadDto>.CreateCreated(newFaq, "FAQ created successfully");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating site FAQ");
//                return ApiResponse<SiteFAQReadDto>.CreateInternalServerError("Internal server error");
//            }
//        }

//        // ===== Product FAQ Endpoints =====

//        [HttpGet("product")]
//        public async Task<ApiResponse<IEnumerable<ProductFAQReadDto>>> GetAllProductFAQs()
//        {
//            using var activity = ActivitySourceProvider.StartActivity();
//            try
//            {
//                var faqs = await _productFAQService.GetAllAsync();
//                return ApiResponse<IEnumerable<ProductFAQReadDto>>.CreateSuccess(faqs);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving product FAQs");
//                return ApiResponse<IEnumerable<ProductFAQReadDto>>.CreateInternalServerError("Internal server error");
//            }
//        }

//        [HttpGet("product/{productId}")]
//        public async Task<ApiResponse<IEnumerable<ProductFAQSimpleDto>>> GetFAQsByProductId(int productId)
//        {
//            using var activity = ActivitySourceProvider.StartActivity();
//            try
//            {
//                var faqs = await _productFAQService.GetByProductIdAsync(productId);
//                return ApiResponse<IEnumerable<ProductFAQSimpleDto>>.CreateSuccess(faqs);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving FAQs for product ID: {ProductId}", productId);
//                return ApiResponse<IEnumerable<ProductFAQSimpleDto>>.CreateInternalServerError("Internal server error");
//            }
//        }

//        [HttpPost("product")]
//        public async Task<ApiResponse<ProductFAQReadDto>> CreateProductFAQ([FromBody] ProductFAQCreateDto dto)
//        {
//            using var activity = ActivitySourceProvider.StartActivity();
//            try
//            {
//                var newFaq = await _productFAQService.AddAsync(dto);
//                return ApiResponse<ProductFAQReadDto>.CreateCreated(newFaq, "FAQ created successfully");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating product FAQ");
//                return ApiResponse<ProductFAQReadDto>.CreateInternalServerError("Internal server error");
//            }
//        }

//        // ===== Unified Endpoints =====

//        [HttpGet("grouped")]
//        public async Task<ApiResponse<FAQCollectionDto>> GetGroupedFAQs()
//        {
//            using var activity = ActivitySourceProvider.StartActivity();
//            try
//            {
//                var faqs = await _productFAQService.GetGroupedFAQsAsync();
//                return ApiResponse<FAQCollectionDto>.CreateSuccess(faqs);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving grouped FAQs");
//                return ApiResponse<FAQCollectionDto>.CreateInternalServerError("Internal server error");
//            }
//        }

//        [HttpDelete("{id}/{type}")]
//        public async Task<ApiResponse> DeleteFAQ(int id, string type)
//        {
//            using var activity = ActivitySourceProvider.StartActivity();
//            try
//            {
//                if (type.Equals("site", StringComparison.OrdinalIgnoreCase))
//                {
//                    await _siteFAQService.DeleteAsync(id);
//                }
//                else if (type.Equals("product", StringComparison.OrdinalIgnoreCase))
//                {
//                    await _productFAQService.DeleteAsync(id);
//                }
//                else
//                {
//                    return ApiResponse.CreateBadRequest("Invalid FAQ type");
//                }

//                return ApiResponse.CreateNoContent();
//            }
//            catch (KeyNotFoundException ex)
//            {
//                _logger.LogWarning(ex, "FAQ not found: {Id}", id);
//                return ApiResponse.CreateNotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting FAQ ID: {Id}", id);
//                return ApiResponse.CreateInternalServerError("Internal server error");
//            }
//        }
//    }
//}
