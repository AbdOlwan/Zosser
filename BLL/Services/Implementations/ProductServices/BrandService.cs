using BLL.Exceptions;
using BLL.Services.Interfaces.ProductInterfaces;
using DAL.Entities.Models.ProductModels;
using DAL.Repositories.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Shared.DTOs.ProductDTOs;
using Shared.Utils;

namespace BLL.Services.Implementations.ProductServices
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandService> _logger;

        public BrandService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<BrandService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BrandResponseDTO> CreateBrandAsync(BrandCreateDTO brandDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var brand = _mapper.Map<Brand>(brandDto);
                var createdBrand = await _unitOfWork.Brands.AddBrandAsync(brand);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<BrandResponseDTO>(createdBrand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل إنشاء العلامة التجارية: {@BrandDto}", brandDto);
                throw new ApplicationException("حدث خطأ أثناء إنشاء العلامة التجارية", ex);
            }
        }

        public async Task<BrandResponseDTO> UpdateBrandAsync(BrandUpdateDTO brandDto)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var existingBrand = await _unitOfWork.Brands.GetBrandByIdAsync(brandDto.Id);
                if (existingBrand == null)
                {
                    _logger.LogWarning("العلامة التجارية غير موجودة: {BrandId}", brandDto.Id);
                    throw new NotFoundException($"العلامة التجارية {brandDto.Id} غير موجودة");
                }
                brandDto.Adapt(existingBrand);
              //  _mapper.Map(brandDto, existingBrand);
                await _unitOfWork.Brands.UpdateBrandAsync(existingBrand);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<BrandResponseDTO>(existingBrand);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل تحديث العلامة التجارية: {@BrandDto}", brandDto);
                throw new ApplicationException("حدث خطأ أثناء تحديث العلامة التجارية", ex);
            }
        }

        public async Task DeleteBrandAsync(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var brand = await _unitOfWork.Brands.GetBrandByIdAsync(id);
                if (brand == null)
                {
                    _logger.LogWarning("العلامة التجارية غير موجودة للحذف: {BrandId}", id);
                    throw new NotFoundException($"العلامة التجارية {id} غير موجودة");
                }

                await _unitOfWork.Brands.DeleteBrandAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل حذف العلامة التجارية: {BrandId}", id);
                throw new ApplicationException("حدث خطأ أثناء حذف العلامة التجارية", ex);
            }
        }

        public async Task<BrandResponseDTO> GetBrandByIdAsync(int id)
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var brand = await _unitOfWork.Brands.GetBrandByIdAsync(id);
                if (brand == null)
                {
                    _logger.LogWarning("العلامة التجارية غير موجودة: {BrandId}", id);
                    throw new NotFoundException($"العلامة التجارية {id} غير موجودة");
                }
                return _mapper.Map<BrandResponseDTO>(brand);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب العلامة التجارية: {BrandId}", id);
                throw new ApplicationException("حدث خطأ أثناء جلب العلامة التجارية", ex);
            }
        }

        public async Task<IEnumerable<BrandResponseDTO>> GetAllBrandsAsync()
        {
            using var activity = ActivitySourceProvider.StartActivity();
            try
            {
                var brands = await _unitOfWork.Brands.GetAllBrandsAsync();
                return _mapper.Map<IEnumerable<BrandResponseDTO>>(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل جلب جميع العلامات التجارية");
                throw new ApplicationException("حدث خطأ أثناء جلب العلامات التجارية", ex);
            }
        }
    }
}
