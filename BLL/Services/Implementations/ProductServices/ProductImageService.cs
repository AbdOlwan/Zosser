using BLL.Services.Interfaces.ProductInterfaces;
using DAL.Entities.Models.ProductModels;
using DAL.Repositories.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Shared.DTOs.ProductDTOs;
using Shared.Interfaces;
using Shared.Utils;

namespace BLL.Services.Implementations.ProductServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductImageService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public ProductImageService(
            IUnitOfWork unitOfWork,
            ILogger<ProductImageService> logger,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ProductImageResponseDTO> AddImage(ProductImageCreateDTO imageDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var image = _mapper.Map<ProductImage>(imageDto);

                // التحقق من وجود المنتج
                if (!await _unitOfWork.Products.ProductExistsAsync(image.ProductId))
                    throw new KeyNotFoundException("المنتج غير موجود");

                var addedImage = await _unitOfWork.ProductImages.AddImageAsync(image);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<ProductImageResponseDTO>(addedImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل إضافة صورة للمنتج: {ProductId}", imageDto.ProductId);
                throw new ApplicationException("حدث خطأ أثناء إضافة الصورة", ex);
            }
        }

        public async Task UpdateImage(ProductImageUpdateDTO imageDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var existingImage = await _unitOfWork.ProductImages.GetImageByIdAsync(imageDto.Id);
                if (existingImage == null)
                    throw new KeyNotFoundException("الصورة غير موجودة");

                _mapper.Map(imageDto, existingImage);
                await _unitOfWork.ProductImages.UpdateImageAsync(existingImage);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل تحديث الصورة: {ImageId}", imageDto.Id);
                throw new ApplicationException("حدث خطأ أثناء تحديث الصورة", ex);
            }
        }

        public async Task DeleteImage(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                await _unitOfWork.ProductImages.DeleteImageAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل حذف الصورة: {ImageId}", id);
                throw new ApplicationException("حدث خطأ أثناء حذف الصورة", ex);
            }
        }

        public async Task<ProductImageResponseDTO?> GetImageById(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var image = await _unitOfWork.ProductImages.GetImageByIdAsync(id);
                return image == null ? null : _mapper.Map<ProductImageResponseDTO>(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب الصورة: {ImageId}", id);
                throw new ApplicationException("حدث خطأ أثناء جلب الصورة", ex);
            }
        }

        public async Task<IEnumerable<ProductImageResponseDTO>> GetImagesByProductId(int productId)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var images = await _unitOfWork.ProductImages.GetImagesByProductIdAsync(productId);
                return _mapper.Map<IEnumerable<ProductImageResponseDTO>>(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب صور المنتج: {ProductId}", productId);
                throw new ApplicationException("حدث خطأ أثناء جلب صور المنتج", ex);
            }
        }
    }
}
