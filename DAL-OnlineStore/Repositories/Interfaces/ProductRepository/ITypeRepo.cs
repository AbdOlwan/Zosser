using DAL_OnlineStore.Entities.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductType = DAL_OnlineStore.Entities.Models.ProductModels.ProductType;

namespace DAL_OnlineStore.Repositories.Interfaces.ProductRepository
{
    public interface ITypeRepo
    {
        Task<List<ProductType>?> getAllTypes();
        Task<ProductType> addNewType(ProductType Type);

        //Task<int> countTypes();

        Task<ProductType?> getTypeById(int id);

        Task<bool> deleteTypeById(int id);

        Task<bool> updateTypeById(ProductType Type);

        Task<bool> TypeExistsAsync(int id);
    }
}
