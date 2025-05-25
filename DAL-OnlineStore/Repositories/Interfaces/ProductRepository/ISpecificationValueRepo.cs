using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
{
    public interface ISpecificationValueRepo
    {
        Task<List<SpecificationValue>?> getAllSpecificationValues();
        Task<SpecificationValue> addNewSpecificationValue(SpecificationValue SpecificationValue);

        //Task<int> countSpecificationValues();

        Task<SpecificationValue?> getSpecificationValueById(int id);

        Task<bool> deleteSpecificationValueById(int id);

        Task<bool> updateSpecificationValueById(SpecificationValue SpecificationValue);
    }
}
