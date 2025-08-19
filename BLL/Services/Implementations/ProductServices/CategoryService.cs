using BLL.Exceptions;
using BLL.Services.Interfaces.ProductInterfaces;
using DAL.Entities.Models.ProductModels;
using DAL.Repositories.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Shared.DTOs.ProductDTOs.General;
using Shared.Utils;

namespace BLL.Services.Implementations.ProductServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryResponseDTO> CreateCategoryAsync(CategoryCreateDTO categoryDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                var createdCategory = await _unitOfWork.Category.AddCategoryAsync(category);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CategoryResponseDTO>(createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل إنشاء الفئة: {@CategoryDto}", categoryDto);
                throw new ApplicationException("حدث خطأ أثناء إنشاء الفئة", ex);
            }
        }

        public async Task<CategoryResponseDTO> UpdateCategoryAsync(CategoryUpdateDTO categoryDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var existingCategory = await _unitOfWork.Category.GetCategoryByIdAsync(categoryDto.Id);
                if (existingCategory == null)
                {
                    _logger.LogWarning("الفئة غير موجودة: {CategoryId}", categoryDto.Id);
                    throw new NotFoundException($"الفئة {categoryDto.Id} غير موجودة");
                }

                _mapper.Map(categoryDto, existingCategory);
                await _unitOfWork.Category.UpdateCategoryAsync(existingCategory);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CategoryResponseDTO>(existingCategory);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل تحديث الفئة: {@CategoryDto}", categoryDto);
                throw new ApplicationException("حدث خطأ أثناء تحديث الفئة", ex);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var category = await _unitOfWork.Category.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("الفئة غير موجودة للحذف: {CategoryId}", id);
                    throw new NotFoundException($"الفئة {id} غير موجودة");
                }

                await _unitOfWork.Category.DeleteCategoryAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل حذف الفئة: {CategoryId}", id);
                throw new ApplicationException("حدث خطأ أثناء حذف الفئة", ex);
            }
        }

        public async Task<CategoryResponseDTO> GetCategoryByIdAsync(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var category = await _unitOfWork.Category.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("الفئة غير موجودة: {CategoryId}", id);
                    throw new NotFoundException($"الفئة {id} غير موجودة");
                }
                return _mapper.Map<CategoryResponseDTO>(category);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب الفئة: {CategoryId}", id);
                throw new ApplicationException("حدث خطأ أثناء جلب الفئة", ex);
            }
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var categories = await _unitOfWork.Category.GetAllCategoriesAsync();
                return _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب جميع الفئات");
                throw new ApplicationException("حدث خطأ أثناء جلب الفئات", ex);
            }
        }
    }
}
