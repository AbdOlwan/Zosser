using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ProductBusServices
{
    public interface ISpecificationServices
    {
        Task<List<SpecificationDTO>?> GetAllSpecifications();
        Task<SpecificationDTO?> AddNewSpecification(SpecificationDTO Specification);

        //Task<int> countSpecifications();

        Task<SpecificationDTO?> GetSpecificationById(int id);

        Task<bool> DeleteSpecificationById(int id);

        Task<bool> UpdateSpecificationById(SpecificationDTO Specification);
    }
}
