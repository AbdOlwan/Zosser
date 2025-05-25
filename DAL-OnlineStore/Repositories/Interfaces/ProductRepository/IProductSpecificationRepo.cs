using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
{
    public interface IProductSpecificationRepo
    {
        Task<List<ProductSpecification>?> getAllProductSpecifications();
        Task<ProductSpecification> addNewProductSpecification(ProductSpecification ProductSpecification);

        //Task<int> countProductSpecifications();

        //    Task<ProductSpecification?> getProductSpecificationById(int id);

        //    Task<bool> deleteProductSpecificationById(int id);

        //    Task<bool> updateProductSpecificationById(ProductSpecification ProductSpecification);
    }
}
