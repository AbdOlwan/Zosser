using BLL.Services.Interfaces.ProductInterfaces;
using DAL.Entities.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.ProductDTOs;
using Shared.Interfaces;
using Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Products
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ICurrentUserService currentUserService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpPost]
       // [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                if (!ModelState.IsValid)
                    return ApiResponse.CreateBadRequest(ModelState).ToActionResult();

                var createdProduct = await _productService.CreateProduct(productDto);
                if (createdProduct == null) 
                    return ApiResponse.CreateBadRequest("خطأ في اضافة منتج : {ProductId}").ToActionResult();

                return ApiResponse<ProductResponseDTO>
                    .CreateCreated(createdProduct, ApiMessages.Created)
                    .ToActionResult();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "خطأ في التحقق: {Message}", ex.Message);
                return ApiResponse.CreateBadRequest(ex.Message).ToActionResult();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "المرجع غير موجود: {Message}", ex.Message);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء إنشاء المنتج");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        /// <summary>
        /// تحديث منتج موجودة
        /// </summary>
        /// <param name="id">معرف المنتج</param>
        [HttpPut("{id}")]
        //  [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct( int id,[FromBody] ProductUpdateDTO productDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                if (!ModelState.IsValid)
                    return ApiResponse.CreateBadRequest(ModelState).ToActionResult();
                if (id != productDto.Id)
                {
                    _logger.LogWarning("Mismatched Product ID in update: Route {RouteId}, Body {BodyId}", id, productDto.Id);
                    return ApiResponse<ProductResponseDTO>
                        .CreateBadRequest("Product ID in route does not match body").ToActionResult();
                }

                var product = await _productService.GetProductById(id);
                if (product == null)
                    return  ApiResponse.CreateBadRequest("المنتج غير موجود: {ProductId}").ToActionResult();

                var updatedProduct = await _productService.UpdateProduct(productDto);
                if(updatedProduct == null) return ApiResponse.CreateBadRequest("المنتج غير موجود: {ProductId}").ToActionResult();

                return ApiResponse<ProductResponseDTO>
                    .CreateSuccess(updatedProduct, ApiMessages.Updated)
                    .ToActionResult();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "المنتج غير موجود: {ProductId}", productDto.Id);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء تحديث المنتج: {ProductId}", productDto.Id);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        [HttpDelete("{id}")]
     //   [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct([Range(1, int.MaxValue)] int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var product = _productService.GetProductById(id);
                if (product == null)
                    return ApiResponse.CreateNotFound("Product Not found").ToActionResult();

                await _productService.DeleteProduct(id);
                return ApiResponse.CreateNoContent(ApiMessages.Deleted).ToActionResult();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "المنتج غير موجود: {ProductId}", id);
                return ApiResponse.CreateNotFound(ex.Message).ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء حذف المنتج: {ProductId}", id);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ProductResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductById([Range(1, int.MaxValue)] int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                    return ApiResponse.CreateNotFound("المنتج غير موجود").ToActionResult();

                return ApiResponse<ProductResponseDTO>
                    .CreateSuccess(product, ApiMessages.Success)
                    .ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء جلب المنتج: {ProductId}", id);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<ProductCardDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductsPaged(
            [Range(1, int.MaxValue)] int pageNumber = 1,
            [Range(1, DefaultValues.MaxPageSize)] int pageSize = DefaultValues.DefaultPageSize)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var pagedProducts = await _productService.GetProductCardsPaged(pageNumber, pageSize);
                return ApiResponse<PagedResponse<ProductCardDto>>
                    .CreateSuccess(pagedProducts, ApiMessages.Success)
                    .ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء جلب المنتجات بصفحة رقم {PageNumber}", pageNumber);
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }

        [HttpGet]
     //   [Authorize(Policy = Policies.RequireAdminRole)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductResponseDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllProducts()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var products = await _productService.GetAllProducts();
                return ApiResponse<IEnumerable<ProductResponseDTO>>
                    .CreateSuccess(products, ApiMessages.Success)
                    .ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ غير متوقع أثناء جلب جميع المنتجات");
                return ApiResponse.CreateInternalServerError(ApiMessages.InternalServerError).ToActionResult();
            }
        }
    }
}

