using BLL.Exceptions;
using BLL.Services.Interfaces.ProductInterfaces;
using DAL.Entities.Models.ProductModels;
using DAL.Repositories.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DTOs.ProductDTOs;
using Shared.Interfaces;
using Shared.Utils;

namespace BLL.Services.Implementations.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public ProductService(
            IUnitOfWork unitOfWork,
            ILogger<ProductService> logger,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ProductResponseDTO?> CreateProduct(ProductCreateDTO productDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var product = productDto.Adapt<Product>();

                // بدء معاملة قاعدة البيانات
                await _unitOfWork.BeginTransactionAsync();

                // إضافة المنتج
                var createdProduct = await _unitOfWork.Products.AddProductAsync(product);
                await _unitOfWork.SaveChangesAsync();

                // إضافة الصور
                //foreach (var imageDto in productDto.Images)
                //{
                //    var image = imageDto.Adapt<ProductImage>();
                //    image.ProductId = createdProduct.Id;
                //    await _unitOfWork.ProductImages.AddImageAsync(image);
                //}

                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.SaveChangesAsync();


                return await GetFullProductResponse(createdProduct.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "فشل إنشاء المنتج: {ProductName}", productDto.Name);
                throw new ApplicationException("حدث خطأ أثناء إنشاء المنتج", ex);
            }
        }

        public async Task<ProductResponseDTO?> UpdateProduct(ProductUpdateDTO productDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var existingProduct = await _unitOfWork.Products.GetProductByIdAsync(productDto.Id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("المنتج غير موجود للتحديث: {ProductId}", productDto.Id);
                    throw new KeyNotFoundException("المنتج غير موجود");
                }

                _mapper.Map(productDto, existingProduct);

                await _unitOfWork.BeginTransactionAsync();

                // تحديث المنتج
                await _unitOfWork.Products.UpdateProductAsync(existingProduct);

                // معالجة الصور (إضافة/تحديث/حذف)
                //await ProcessProductImages(productDto, existingProduct);

                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.SaveChangesAsync();

                return await GetFullProductResponse(existingProduct.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "فشل تحديث المنتج: {ProductId}", productDto.Id);
                throw new ApplicationException("حدث خطأ أثناء تحديث المنتج", ex);
            }
        }



        public async Task DeleteProduct(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                await _unitOfWork.Products.DeleteProductAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل حذف المنتج: {ProductId}", id);
                throw new ApplicationException("حدث خطأ أثناء حذف المنتج", ex);
            }
        }

        public async Task<ProductResponseDTO?> GetProductById(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                return await GetFullProductResponse(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب المنتج: {ProductId}", id);
                throw new ApplicationException("حدث خطأ أثناء جلب بيانات المنتج", ex);
            }
        }

        private async Task<ProductResponseDTO?> GetFullProductResponse(int productId)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(productId);
            if (product == null) return null;

            var response = _mapper.Map<ProductResponseDTO>(product);



            return response;
        }

        public async Task<PagedResponse<ProductCardDto>> GetProductCardsPaged(int pageNumber, int pageSize)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var (products, totalCount) = await _unitOfWork.Products.GetProductCardsPagedAsync(
                    pageNumber,
                    pageSize);

                return new PagedResponse<ProductCardDto>
                {
                    Items = products,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب بطاقات المنتجات");
                throw new ApplicationException("حدث خطأ أثناء جلب بطاقات المنتجات", ex);
            }
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllProducts()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var products = await _unitOfWork.Products.GetAllProductsAsync();
                return _mapper.Map<IEnumerable<ProductResponseDTO>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب جميع المنتجات");
                throw new ApplicationException("حدث خطأ أثناء جلب جميع المنتجات", ex);
            }
        }


        //private async Task ProcessProductImages(ProductUpdateDTO productDto, Product existingProduct)
        //{
        //    // الصور الجديدة
        //    foreach (var imageDto in productDto.Images?.Where(i => i.Id == 0) ?? [])
        //    {
        //        // var image = _mapper.Map<ProductImage>(imageDto);
        //        var image = imageDto.Adapt<ProductImage>();
        //        image.ProductId = existingProduct.Id;
        //        await _unitOfWork.ProductImages.AddImageAsync(image);
        //    }

        //    // الصور المحدثة
        //    foreach (var imageDto in productDto.Images?.Where(i => i.Id > 0) ?? [])
        //    {
        //        var existingImage = await _unitOfWork.ProductImages.GetImageByIdAsync(imageDto.Id);
        //        if (existingImage != null)
        //        {
        //            _mapper.Map(imageDto, existingImage);
        //            await _unitOfWork.ProductImages.UpdateImageAsync(existingImage);
        //        }
        //    }

        //    // الصور المحذوفة (إذا أرسيت قائمة صور محدثة فقط)
        //    var existingImageIds = existingProduct.Images.Select(i => i.Id).ToList();
        //    var updatedImageIds = productDto.Images?.Select(i => i.Id) ?? [];
        //    var imagesToDelete = existingImageIds.Except(updatedImageIds);

        //    foreach (var imageId in imagesToDelete)
        //    {
        //        await _unitOfWork.ProductImages.DeleteImageAsync(imageId);
        //    }
        //}
    }
}
