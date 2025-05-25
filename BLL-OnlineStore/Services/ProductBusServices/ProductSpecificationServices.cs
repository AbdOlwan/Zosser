using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using BLL_OnlineStore.Interfaces.ProductBusServices;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Repositories.Interfaces.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.ProductBusServices
{
    public class ProductSpecificationServices : IProductSpecificationServices
    {


        private readonly IProductSpecificationRepo _repo;
        private readonly IMapper _mapper;

        public ProductSpecificationServices(IProductSpecificationRepo repo, IMapper maper)
        {
            _repo = repo;
            _mapper = maper;
        }

        public async Task<List<ProductSpecificationDTO>?> GetAllProductSpecifications()
        {
            var ProductSpecifications = await _repo.getAllProductSpecifications();
            if (ProductSpecifications == null)
                return null;

            return _mapper.Map<List<ProductSpecificationDTO>>(ProductSpecifications);
        }


        public async Task<ProductSpecificationDTO?> AddNewProductSpecification(ProductSpecificationDTO DTO)
        {
            var _ProductSpecification = _mapper.Map<ProductSpecification>(DTO);

            var NewProductSpecification = await _repo.addNewProductSpecification(_ProductSpecification);
            if (NewProductSpecification != null)
            {
                return _mapper.Map<ProductSpecificationDTO?>(NewProductSpecification);
            }
            return null;
        }
    }
}
