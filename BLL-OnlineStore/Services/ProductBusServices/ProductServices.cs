using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Services.Interfaces;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepo productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(string culture = "ar")
        {
            var products = await _productRepo.GetAllProductsAsync(culture);

            var context = new Dictionary<string, object>
            {
                { "Culture", culture }
            };

            return _mapper.Map<IEnumerable<ProductDTO>>(products, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id, string culture = "ar")
        {
            var product = await _productRepo.GetProductByIdAsync(id, culture);
            if (product == null)
                return null;

            return _mapper.Map<ProductDTO>(product, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<ProductDTO?> GetProductBySlugAsync(string slug, string culture = "ar")
        {
            var product = await _productRepo.GetProductBySlugAsync(slug, culture);
            if (product == null)
                return null;

            return _mapper.Map<ProductDTO>(product, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(int categoryId, string culture = "ar")
        {
            var products = await _productRepo.GetProductsByCategoryIdAsync(categoryId, culture);

            return _mapper.Map<IEnumerable<ProductDTO>>(products, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByBrandIdAsync(int brandId, string culture = "ar")
        {
            var products = await _productRepo.GetProductsByBrandIdAsync(brandId, culture);

            return _mapper.Map<IEnumerable<ProductDTO>>(products, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByTypeIdAsync(int typeId, string culture = "ar")
        {
            var products = await _productRepo.GetProductsByTypeIdAsync(typeId, culture);

            return _mapper.Map<IEnumerable<ProductDTO>>(products, opts =>
            {
                opts.Items["Culture"] = culture;
            });
        }

        public async Task<ProductWithTranslationsDTO?> GetProductWithTranslationsAsync(int id)
        {
            // Get product with all translations
            var product = await _productRepo.GetProductByIdAsync(id, null);
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
            await _productRepo.AddProductAsync(product);

            // Return mapped DTO with default culture
            return await GetProductByIdAsync(product.ProductId);
        }

        public async Task UpdateProductAsync(UpdateProductDTO updateProductDTO)
        {
            // Get existing product with all translations
            var product = await _productRepo.GetProductByIdAsync(updateProductDTO.ProductId, null);
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
            await _productRepo.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
             await _productRepo.DeleteProductAsync(id);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _productRepo.ProductExistsAsync(id);
        }

        public async Task<bool> ProductExistsBySlugAsync(string slug)
        {
            return await _productRepo.ProductExistsBySlugAsync(slug);
        }
    }
}


























//using AutoMapper;
//using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
//using BLL_OnlineStore.Interfaces;
//using BLL_OnlineStore.Interfaces.ProductBusServices;
//using DAL_OnlineStore.Entities.Models.ProductModels;
//using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;

//namespace BLL_OnlineStore.Services.ProductBusServices
//{
//    public class ProductServices : IProductServices
//    {
//        private readonly IProductRepo _repo;
//        private readonly IMapper _mapper;
//        private readonly ICultureService _culture;
//        public ProductServices
//            (IProductRepo repo,
//            IMapper mapper,
//            ICultureService culture)
//        {
//            _repo = repo;
//            _mapper = mapper;
//            _culture = culture;
//        }

//        public async Task<List<ProductDTO>?> GetAllProducts()
//        {
//            var Products = await _repo.getAllProducts();
//            if (Products == null)
//                return null;

//            return _mapper.Map<List<ProductDTO>>(Products);

//        }

//        public async Task<ProductDTO?> GetProductById(int ID)
//        {
//            var Product = await _repo.getProductById(ID);
//            if (Product == null) return null;

//            return _mapper.Map<ProductDTO>(Product);
//        }

//        //public async Task<ProductDTO?> AddNewProduct(ProductDTO DTO)
//        //{
//        //    var Product = _mapper.Map<Product>(DTO);

//        //    var Newproduct = await _repo.addNewProduct(Product);
//        //    if (Newproduct != null)
//        //    {
//        //        return _mapper.Map<ProductDTO?>(Newproduct);
//        //    }
//        //    return null;
//        //}



//        public async Task<ProductDTO?> AddNewProduct(ProductCreateUpdateDto dto)
//        {
//            // استخدم AutoMapper لإنشاء الكيان بالكامل بما في ذلك الترجمات
//            var entity = _mapper.Map<Product>(dto);

//            // ضبط الحقول التلقائية
//            entity.CreatedAt = DateTime.UtcNow;
//            entity.UpdatedAt = DateTime.UtcNow;

//            var Newproduct = await _repo.addNewProduct(entity);
//            if (Newproduct != null)
//            {
//                return _mapper.Map<ProductDTO?>(entity);
//            }
//            return null;

//        }


//        public async Task<ProductDTO?> UpdateProductById(int id, ProductCreateUpdateDto dto)
//        {
//            if (dto == null)
//                return null;
//            var entity = await _repo.getProductById(id);
//            if (entity == null) return null;

//            // استخدم AutoMapper لتحديث الحقول بما في ذلك قائمة الترجمات
//            _mapper.Map(dto, entity);

//            // ضبط الحقول التلقائية
//            entity.UpdatedAt = DateTime.UtcNow;

//            var newEntity = await _repo.updateProduct(entity);

//            return _mapper.Map<ProductDTO>(newEntity);
//        }


//        public async Task<bool> DeleteProductById(int id)
//        {
//            return await _repo.deleteProductById(id);
//        }

//        public async Task<int> CountProducts()
//        {
//            return await _repo.countProducts();
//        }
//    }
//}
