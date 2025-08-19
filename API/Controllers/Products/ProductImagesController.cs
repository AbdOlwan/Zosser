// ProductImagesController.cs
using BLL.Services.Interfaces.ProductInterfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.ProductDTOs;
using Shared.Interfaces;
using Shared.Utils;

namespace API.Controllers.Products
{
    [ApiController]
    [Route("api/v1/product-images")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _imageService;
        private readonly ILogger<ProductImagesController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public ProductImagesController(
            IProductImageService imageService,
            ILogger<ProductImagesController> logger,
            ICurrentUserService currentUserService)
        {
            _imageService = imageService;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ProductImageResponseDTO>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> AddImage([FromBody] ProductImageCreateDTO imageDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var result = await _imageService.AddImage(imageDto);
                return ApiResponse<ProductImageResponseDTO>
                    .CreateCreated(result, "تم إضافة الصورة بنجاح")
                    .ToActionResult();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "خطأ في إضافة الصورة: {Message}", ex.Message);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء إضافة الصورة");
                return ApiResponse.CreateInternalServerError("حدث خطأ أثناء معالجة طلبك").ToActionResult();
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> UpdateImage([FromBody] ProductImageUpdateDTO imageDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var image = await _imageService.GetImageById(imageDto.Id); // AWAIT added here
                if (image == null)
                    return ApiResponse.CreateNotFound("الصورة غير موجوده").ToActionResult();

                await _imageService.UpdateImage(imageDto);
                return ApiResponse.CreateSuccess("تم تحديث الصورة بنجاح").ToActionResult();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "خطأ في تحديث الصورة: {Message}", ex.Message);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء تحديث الصورة");
                return ApiResponse.CreateInternalServerError("حدث خطأ أثناء معالجة طلبك").ToActionResult();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> DeleteImage(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var image = await _imageService.GetImageById(id); // AWAIT added here
                if (image == null)
                    return ApiResponse.CreateNotFound("الصورة غير موجوده").ToActionResult();

                await _imageService.DeleteImage(id);
                return ApiResponse.CreateSuccess("تم حذف الصورة بنجاح").ToActionResult();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "خطأ في حذف الصورة: {Message}", ex.Message);
                return ApiResponse.CreateConflict(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء حذف الصورة");
                return ApiResponse.CreateInternalServerError("حدث خطأ أثناء معالجة طلبك").ToActionResult();
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProductImageResponseDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<IActionResult> GetImageById(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var image = await _imageService.GetImageById(id);
                if (image == null)
                    return ApiResponse.CreateNotFound("الصورة غير موجودة").ToActionResult();

                return ApiResponse<ProductImageResponseDTO>
                    .CreateSuccess(image, "تم جلب الصورة بنجاح")
                    .ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء جلب الصورة");
                return ApiResponse.CreateInternalServerError("حدث خطأ أثناء معالجة طلبك").ToActionResult();
            }
        }

        [HttpGet("product/{productId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductImageResponseDTO>>), 200)]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var images = await _imageService.GetImagesByProductId(productId);
                return ApiResponse<IEnumerable<ProductImageResponseDTO>>
                    .CreateSuccess(images, "تم جلب صور المنتج بنجاح")
                    .ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء جلب صور المنتج");
                return ApiResponse.CreateInternalServerError("حدث خطأ أثناء معالجة طلبك").ToActionResult();
            }
        }
    }
}