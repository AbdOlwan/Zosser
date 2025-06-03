using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Services.Interfaces;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories;
using DAL_OnlineStore.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepo productRepo, IMapper mapper)
        {
            _repo = productRepo;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDTO>> GetAllProductsAsync(string culture = "ar", int page = 1, int limit = 10)
        {
            var pagedProducts = await _repo.GetAllProductsAsync(culture, page, limit);

            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(pagedProducts.Items, opts =>
            {
                opts.Items["Culture"] = culture;
            });

            return new PagedResult<ProductDTO>
            {
                Items = productDTOs,
                TotalPages = pagedProducts.TotalPages,
                CurrentPage = pagedProducts.CurrentPage,
                PageSize = pagedProducts.PageSize,
                TotalItems = pagedProducts.TotalItems
            };
        }


        public async Task<ProductDTO?> GetProductByIdAsync(int id, string culture = "ar")
        {
            var product = await _repo.GetProductByIdAsync(id, culture);
            if (product == null)
                return null;

            return _mapper.Map<ProductDTO>(product, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<ProductDTO?> GetProductBySlugAsync(string slug, string culture = "ar")
        {
            var product = await _repo.GetProductBySlugAsync(slug, culture);
            if (product == null)
                return null;

            return _mapper.Map<ProductDTO>(product, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<PagedResult<ProductDTO>> GetProductsByCategoryIdAsync(int categoryId, string culture = "ar", int page = 1, int limit = 10)
        {
            var result = await _repo.GetProductsByCategoryIdAsync(categoryId, culture, page, limit);

            var mapped = _mapper.Map<IEnumerable<ProductDTO>>(result.Items, opts =>
            {
                opts.Items["Culture"] = culture;
            });

            return new PagedResult<ProductDTO>
            {
                Items = mapped,
                TotalPages = result.TotalPages,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,         // ✅ انسخه من النتيجة الأصلية
                TotalItems = result.TotalItems      // ✅ انسخه من النتيجة الأصلية
            };
        }


        public async Task<PagedResult<ProductDTO>> GetProductsByBrandIdAsync(int brandId, string culture = "ar", int page = 1, int limit = 10)
        {
            var result = await _repo.GetProductsByBrandIdAsync(brandId, culture, page, limit);

            var dtoItems = _mapper.Map<IEnumerable<ProductDTO>>(result.Items, opts =>
            {
                opts.Items["Culture"] = culture;
            });

            return new PagedResult<ProductDTO>
            {
                Items = dtoItems,
                TotalPages = result.TotalPages,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,         // ✅ انسخه من النتيجة الأصلية
                TotalItems = result.TotalItems      // ✅ انسخه من النتيجة الأصلية
            };

        }


        public async Task<PagedResult<ProductDTO>> GetProductsByTypeIdAsync(int typeId, string culture = "ar", int page = 1, int limit = 10)
        {
            var result = await _repo.GetProductsByTypeIdAsync(typeId, culture, page, limit);

            var items =  _mapper.Map<IEnumerable<ProductDTO>>(result.Items, opts =>
            {
                opts.Items["Culture"] = culture;
            });

            return new PagedResult<ProductDTO>
            {
                Items = items,
                TotalPages = result.TotalPages,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,         
                TotalItems = result.TotalItems      
            };
        }

        public async Task<ProductWithTranslationsDTO?> GetProductWithTranslationsAsync(int id)
        {
            // Get product with all translations
            var product = await _repo.GetProductByIdAsync(id, null);
            if (product == null)
                return null;

            // Map to DTO with all translations
            var productWithTranslations = _mapper.Map<ProductWithTranslationsDTO>(product);

            // Map all translations separately
            productWithTranslations.Translations = _mapper.Map<List<ProductTranslationDTO>>(product.productTranslations);

            return productWithTranslations;
        }

        public async Task<ProductDTO?> CreateProductAsync(CreateProductDTO createProductDTO)
        {
            // Map base product data
            var product = _mapper.Map<Product>(createProductDTO);

            // Create translations for Arabic and English
            product.productTranslations = new List<ProductTranslation>
            {
                new ProductTranslation
                {
                    Culture = "ar",
                    ProductName = createProductDTO.ArProductName,
                    Description = createProductDTO.ArDescription,
                    MarketingSlogan = createProductDTO.ArMarketingSlogan
                },
                new ProductTranslation
                {
                    Culture = "en",
                    ProductName = createProductDTO.EnProductName,
                    Description = createProductDTO.EnDescription,
                    MarketingSlogan = createProductDTO.EnMarketingSlogan
                }
            };

            // Save to database
            await _repo.AddProductAsync(product);

            // Return mapped DTO with default culture
            return await GetProductByIdAsync(product.ProductId);
        }

        public async Task UpdateProductAsync(UpdateProductDTO updateProductDTO)
        {
            // Get existing product with all translations
            var product = await _repo.GetProductByIdAsync(updateProductDTO.ProductId, null);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {updateProductDTO.ProductId} not found");

            // Update base product properties
            _mapper.Map(updateProductDTO, product);

            // Update translations
            var arTranslation = product.productTranslations.FirstOrDefault(pt => pt.Culture == "ar");
            var enTranslation = product.productTranslations.FirstOrDefault(pt => pt.Culture == "en");

            if (arTranslation != null)
            {
                arTranslation.ProductName = updateProductDTO.ArProductName;
                arTranslation.Description = updateProductDTO.ArDescription;
                arTranslation.MarketingSlogan = updateProductDTO.ArMarketingSlogan;
            }
            else
            {
                product.productTranslations.Add(new ProductTranslation
                {
                    Culture = "ar",
                    ProductName = updateProductDTO.ArProductName,
                    Description = updateProductDTO.ArDescription,
                    MarketingSlogan = updateProductDTO.ArMarketingSlogan
                });
            }

            if (enTranslation != null)
            {
                enTranslation.ProductName = updateProductDTO.EnProductName;
                enTranslation.Description = updateProductDTO.EnDescription;
                enTranslation.MarketingSlogan = updateProductDTO.EnMarketingSlogan;
            }
            else
            {
                product.productTranslations.Add(new ProductTranslation
                {
                    Culture = "en",
                    ProductName = updateProductDTO.EnProductName,
                    Description = updateProductDTO.EnDescription,
                    MarketingSlogan = updateProductDTO.EnMarketingSlogan
                });
            }

            // Save changes
            await _repo.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
             await _repo.DeleteProductAsync(id);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _repo.ProductExistsAsync(id);
        }

        public async Task<bool> ProductExistsBySlugAsync(string slug)
        {
            return await _repo.ProductExistsBySlugAsync(slug);
        }
    }
}
