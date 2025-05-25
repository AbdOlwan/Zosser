using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ProductBusServices
{
    public interface IProductSpecificationServices
    {
        Task<List<ProductSpecificationDTO>?> GetAllProductSpecifications();
        Task<ProductSpecificationDTO?> AddNewProductSpecification(ProductSpecificationDTO DTO);
    }
}
