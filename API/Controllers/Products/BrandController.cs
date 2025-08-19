
using BLL.Exceptions;
using BLL.Services.Interfaces.ProductInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.ProductDTOs;
using Shared.Utils;

namespace API.Controllers.Products
{
    [Route("api/v1/Brand")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly ILogger<BrandController> _logger;

        public BrandController(
            IBrandService brandService,
            ILogger<BrandController> logger)
        {
            _brandService = brandService;
            _logger = logger;
        }

        /// <summary>
        /// الحصول على جميع الماركات
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<IEnumerable<BrandResponseDTO>>> GetAllBrands()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            using (_logger.BeginScope("GET All Brands"))
            {
                try
                {
                    var brands = await _brandService.GetAllBrandsAsync();
                    return ApiResponse<IEnumerable<BrandResponseDTO>>.CreateSuccess(brands);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving all brands");
                    return ApiResponse<IEnumerable<BrandResponseDTO>>
                        .CreateInternalServerError(ex.Message)
                        .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                }
            }
        }

        /// <summary>
        /// الحصول على ماركة محددة بواسطة المعرف
        /// </summary>
        /// <param name="id">معرف الماركة</param>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ApiResponse<BrandResponseDTO>> GetBrandById(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            using (_logger.BeginScope($"GET Brand by ID: {id}"))
            {
                try
                {
                    var brand = await _brandService.GetBrandByIdAsync(id);
                    return ApiResponse<BrandResponseDTO>.CreateSuccess(brand);
                }
                catch (NotFoundException ex)
                {
                    _logger.LogWarning(ex, "Brand not found: ID {BrandId}", id);
                    return ApiResponse<BrandResponseDTO>
                        .CreateNotFound(ex.Message)
                        .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving brand ID {BrandId}", id);
                    return ApiResponse<BrandResponseDTO>
                        .CreateInternalServerError(ex.Message)
                        .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                }
            }
        }

        /// <summary>
        /// إنشاء ماركة جديدة
        /// </summary>
        [HttpPost]
        // [Authorize(Roles = "Admin,Manager")]
        public async Task<ApiResponse<BrandResponseDTO>> CreateBrand([FromBody] BrandCreateDTO brandDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            using (_logger.BeginScope($"CREATE Brand: {brandDto.Name}"))
            {
                try
                {
                    var createdBrand = await _brandService.CreateBrandAsync(brandDto);
                    return ApiResponse<BrandResponseDTO>
                        .CreateCreated(createdBrand, "Brand created successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating brand: {BrandName}", brandDto.Name);
                    return ApiResponse<BrandResponseDTO>
                        .CreateInternalServerError($"Failed to create brand: {ex.Message}")
                        .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                }
            }
        }

        /// <summary>
        /// تحديث ماركة موجودة
        /// </summary>
        /// <param name="id">معرف الماركة</param>
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin,Manager")]
        public async Task<ApiResponse<BrandResponseDTO>> UpdateBrand(int id, [FromBody] BrandUpdateDTO brandDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            using (_logger.BeginScope($"UPDATE Brand ID: {id}"))
            {
                try
                {
                    // التحقق من تطابق المعرف في المسار مع المعرف في الجسم
                    if (id != brandDto.Id)
                    {
                        _logger.LogWarning("Mismatched brand ID in update: Route {RouteId}, Body {BodyId}", id, brandDto.Id);
                        return ApiResponse<BrandResponseDTO>
                            .CreateBadRequest("Brand ID in route does not match body")
                            .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                    }

                    var updatedBrand = await _brandService.UpdateBrandAsync(brandDto);
                    return ApiResponse<BrandResponseDTO>
                        .CreateSuccess(updatedBrand, "Brand updated successfully");
                }
                catch (NotFoundException ex)
                {
                    _logger.LogWarning(ex, "Brand not found for update: ID {BrandId}", id);
                    return ApiResponse<BrandResponseDTO>
                        .CreateNotFound(ex.Message)
                        .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating brand ID {BrandId}", id);
                    return ApiResponse<BrandResponseDTO>
                        .CreateInternalServerError($"Failed to update brand: {ex.Message}")
                        .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                }
            }
        }

        /// <summary>
        /// حذف ماركة
        /// </summary>
        /// <param name="id">معرف الماركة</param>
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<ApiResponse> DeleteBrand(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            using (_logger.BeginScope($"DELETE Brand ID: {id}"))
            {
                try
                {
                    await _brandService.DeleteBrandAsync(id);
                    return ApiResponse.CreateSuccess("Brand deleted successfully");
                }
                catch (NotFoundException ex)
                {
                    _logger.LogWarning(ex, "Brand not found for deletion: ID {BrandId}", id);
                    return ApiResponse.CreateNotFound(ex.Message)
                        .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting brand ID {BrandId}", id);
                    return ApiResponse.CreateInternalServerError($"Failed to delete brand: {ex.Message}")
                        .WithRequestId(activity?.Id ?? string.Empty); // حل التحذير هنا
                }
            }
        }
    }
}
