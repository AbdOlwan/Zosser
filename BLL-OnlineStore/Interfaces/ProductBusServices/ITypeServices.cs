using BLL_OnlineStore.DTOs.EntitiesDTOs.Product_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ProductBusServices
{
    public interface ITypeServices
    {
        Task<List<TypeDTO>?> GetAllTypes();
        Task<TypeDTO?> AddNewType(TypeDTO Type);

        //Task<int> countTypes();

        Task<TypeDTO?> GetTypeById(int id);

        Task<bool> DeleteTypeById(int id);

        Task<bool> UpdateTypeById(TypeDTO Type);
    }
}
