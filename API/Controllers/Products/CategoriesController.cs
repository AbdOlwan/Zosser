using BLL.Exceptions;
using BLL.Services.Interfaces.ProductInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.DTOs.ProductDTOs.General;
using System.Diagnostics;

namespace API.Controllers.Products
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;
        private readonly ActivitySource _activitySource;

        public CategoriesController(
            ICategoryService categoryService,
            ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
            _activitySource = new ActivitySource("Zosser.Categories");
        }

        /// <summary>
        /// الحصول على جميع الفئات الرئيسية
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
       // [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ApiResponse<IEnumerable<CategoryResponseDTO>>> GetAllCategories()
        {
            using var activity = _activitySource.StartActivity(nameof(GetAllCategories));
            activity?.AddTag("controller", nameof(CategoriesController));

            try
            {
                var stopwatch = Stopwatch.StartNew();
                var categories = await _categoryService.GetAllCategoriesAsync();
                stopwatch.Stop();

                activity?.AddTag("elapsed_time", stopwatch.ElapsedMilliseconds);
                _logger.LogInformation("تم جلب {Count} فئة في {Time} مللي ثانية",
                    categories.Count(), stopwatch.ElapsedMilliseconds);

                return ApiResponse<IEnumerable<CategoryResponseDTO>>.CreateSuccess(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في جلب الفئات: {Message}", ex.Message);
                return ApiResponse<IEnumerable<CategoryResponseDTO>>
                    .CreateInternalServerError(ApiMessages.InternalServerError);
            }
        }

        /// <summary>
        /// الحصول على فئة محددة بواسطة المعرف
        /// </summary>
        /// <param name="id">معرف الفئة</param>
        [HttpGet("{id}")]
        [AllowAnonymous]
      //  [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "id" })]
        public async Task<ApiResponse<CategoryResponseDTO>> GetCategoryById(int id)
        {
            using var activity = _activitySource.StartActivity(nameof(GetCategoryById));
            activity?.AddTag("category_id", id);

            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return ApiResponse<CategoryResponseDTO>.CreateSuccess(category);
            }
            catch (NotFoundException )
            {
                _logger.LogWarning("الفئة غير موجودة: {CategoryId}", id);
                return ApiResponse<CategoryResponseDTO>.CreateNotFound(
                    $"{ApiMessages.NotFound} - الفئة {id} غير موجودة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في جلب الفئة {CategoryId}: {Message}", id, ex.Message);
                return ApiResponse<CategoryResponseDTO>
                    .CreateInternalServerError(ApiMessages.InternalServerError);
            }
        }



        /// <summary>
        /// إنشاء فئة جديدة
        /// </summary>
        [HttpPost]
      //  [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
        public async Task<ApiResponse<CategoryResponseDTO>> CreateCategory(
            [FromBody] CategoryCreateDTO categoryDto)
        {
            using var activity = _activitySource.StartActivity(nameof(CreateCategory));
            activity?.AddTag("category_name", categoryDto.Name);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("بيانات غير صالحة لإنشاء الفئة: {@CategoryDto}", categoryDto);
                return ApiResponse<CategoryResponseDTO>.CreateBadRequest(
                    ValidationMessages.Required);
            }

            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(categoryDto);
                return ApiResponse<CategoryResponseDTO>.CreateCreated(
                    createdCategory,
                    $"{ApiMessages.Created} - تم إنشاء الفئة بنجاح");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء الفئة: {Message}", ex.Message);
                return ApiResponse<CategoryResponseDTO>
                    .CreateInternalServerError($"{ApiMessages.InternalServerError} - فشل إنشاء الفئة");
            }
        }

        /// <summary>
        /// تحديث فئة موجودة
        /// </summary>
        /// <param name="id">معرف الفئة</param>
        [HttpPut("{id}")]
      //  [Authorize(Roles = $"{Roles.Admin},{Roles.Manager}")]
        public async Task<ApiResponse<CategoryResponseDTO>> UpdateCategory(
            int id,
            [FromBody] CategoryUpdateDTO categoryDto)
        {
            using var activity = _activitySource.StartActivity(nameof(UpdateCategory));
            activity?.AddTag("category_id", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("بيانات غير صالحة لتحديث الفئة: {@CategoryDto}", categoryDto);
                return ApiResponse<CategoryResponseDTO>.CreateBadRequest(
                    ValidationMessages.Required);
            }

            if (id != categoryDto.Id)
            {
                _logger.LogWarning("تعارض في معرف الفئة: {RouteId} vs {BodyId}", id, categoryDto.Id);
                return ApiResponse<CategoryResponseDTO>.CreateBadRequest(
                    $"{ApiMessages.BadRequest} - معرف الفئة في المسار لا يتطابق مع الجسم");
            }

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(categoryDto);
                return ApiResponse<CategoryResponseDTO>.CreateSuccess(
                    updatedCategory,
                    $"{ApiMessages.Updated} - تم تحديث الفئة بنجاح");
            }
            catch (NotFoundException )
            {
                _logger.LogWarning("الفئة غير موجودة للتحديث: {CategoryId}", id);
                return ApiResponse<CategoryResponseDTO>.CreateNotFound(
                    $"{ApiMessages.NotFound} - الفئة {id} غير موجودة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تحديث الفئة {CategoryId}: {Message}", id, ex.Message);
                return ApiResponse<CategoryResponseDTO>
                    .CreateInternalServerError($"{ApiMessages.InternalServerError} - فشل تحديث الفئة");
            }
        }

        /// <summary>
        /// حذف فئة
        /// </summary>
        /// <param name="id">معرف الفئة</param>
        [HttpDelete("{id}")]
      //  [Authorize(Roles = Roles.Admin)]
        public async Task<ApiResponse> DeleteCategory(int id)
        {
            using var activity = _activitySource.StartActivity(nameof(DeleteCategory));
            activity?.AddTag("category_id", id);

            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return ApiResponse.CreateSuccess(
                    $"{ApiMessages.Deleted} - تم حذف الفئة بنجاح");
            }
            catch (NotFoundException )
            {
                _logger.LogWarning("الفئة غير موجودة للحذف: {CategoryId}", id);
                return ApiResponse.CreateNotFound(
                    $"{ApiMessages.NotFound} - الفئة {id} غير موجودة");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في حذف الفئة {CategoryId}: {Message}", id, ex.Message);
                return ApiResponse.CreateInternalServerError(
                    $"{ApiMessages.InternalServerError} - فشل حذف الفئة");
            }
        }
    }
}
#region GetSubcategories
///// <summary>
///// الحصول على الفئات الفرعية لفئة محددة
///// </summary>
///// <param name="parentId">معرف الفئة الأب</param>
//[HttpGet("{parentId}/subcategories")]
//[AllowAnonymous]
//[ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "parentId" })]
//public async Task<ApiResponse<IEnumerable<CategoryResponseDTO>>> GetSubcategories(int parentId)
//{
//    using var activity = _activitySource.StartActivity(nameof(GetSubcategories));
//    activity?.AddTag("parent_id", parentId);

//    try
//    {
//        var subcategories = await _categoryService.GetSubcategoriesAsync(parentId);
//        return ApiResponse<IEnumerable<CategoryResponseDTO>>.CreateSuccess(subcategories);
//    }
//    catch (NotFoundException ex)
//    {
//        _logger.LogWarning("الفئة الأب غير موجودة: {ParentId}", parentId);
//        return ApiResponse<IEnumerable<CategoryResponseDTO>>.CreateNotFound(
//            $"{ApiMessages.NotFound} - الفئة الأب {parentId} غير موجودة");
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, "خطأ في جلب الفئات الفرعية للفئة {ParentId}: {Message}",
//            parentId, ex.Message);
//        return ApiResponse<IEnumerable<CategoryResponseDTO>>
//            .CreateInternalServerError(ApiMessages.InternalServerError);
//    }
//}
#endregion