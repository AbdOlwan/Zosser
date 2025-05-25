using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
{
    public interface ISpecificationRepo
    {
        Task<List<Specification>?> getAllSpecifications();
        Task<Specification> addNewSpecification(Specification Specification);

        //Task<int> countSpecifications();

        Task<Specification?> getSpecificationById(int id);

        Task<bool> deleteSpecificationById(int id);

        Task<bool> updateSpecificationById(Specification Specification);
    }
}
