using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ProductBusServices
{
    public interface ISpecificationValueServices
    {
        Task<List<SpecificationValueDTO>?> GetAllSpecificationValues();
        Task<SpecificationValueDTO?> AddNewSpecificationValue(SpecificationValueDTO SpecificationValue);

        //Task<int> countSpecificationValues();

        Task<SpecificationValueDTO?> GetSpecificationValueById(int id);

        Task<bool> DeleteSpecificationValueById(int id);

        Task<bool> UpdateSpecificationValueById(SpecificationValueDTO SpecificationValue);
    }
}
