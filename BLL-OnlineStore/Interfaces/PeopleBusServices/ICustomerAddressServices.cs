using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.PeopleBusServices
{
    public interface ICustomerAddressServices
    {
        Task<List<CustomerAddressDTO>?> GetAllCustomerAddresses();
        Task<CustomerAddressDTO?> AddNewCustomerAddress(CustomerAddressDTO customerAddress);

        //Task<int> countCustomers();

        Task<CustomerAddressDTO?> GetCustomerAddressById(int id);

        Task<bool> DeleteCustomerAddressById(int id);

        Task<bool> UpdateCustomerAddressById(CustomerAddressDTO customerAddress);
    }
}
