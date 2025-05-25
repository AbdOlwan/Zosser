using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using BLL_OnlineStore.Interfaces.ProductBusServices;

namespace BLL_OnlineStore.Services.ProductBusServices
{
    public class ProductImageServices : IProductImageServices
    {
        private readonly IProductImageRepo _repo;
        private readonly IMapper _mapper;

        public ProductImageServices(IProductImageRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ProductImageDTO>?> GetAllProductImages()
        {
            var ProductImages = await _repo.getAllProductImages();
            if (ProductImages == null)
                return null;

            return _mapper.Map<List<ProductImageDTO>>(ProductImages);

        }

        public async Task<ProductImageDTO?> GetProductImageById(int ID)
        {
            var ProductImage = await _repo.getProductImageById(ID);
            if (ProductImage == null) return null;

            return _mapper?.Map<ProductImageDTO>(ProductImage);
        }

        public async Task<ProductImageDTO?> AddNewProductImage(ProductImageDTO DTO)
        {
            var ProductImage = _mapper.Map<ProductImage>(DTO);

            var NewProductImage = await _repo.addNewProductImage(ProductImage);
            if (NewProductImage != null)
            {
                return _mapper.Map<ProductImageDTO>(NewProductImage);
            }
            return null;
        }
        public async Task<bool> UpdateProductImageById(ProductImageDTO DTO)
        {
            if (DTO == null)
                return false;

            var ProductImage = _mapper.Map<ProductImage>(DTO);
            return await _repo.updateProductImageById(ProductImage);
        }
        public async Task<bool> DeleteProductImageById(int id)
        {
            return await _repo.deleteProductImageById(id);
        }

        public async Task<int> CountProductImages(int ProductId)
        {
            return await _repo.countProductImages(ProductId);
        }


    }
}
