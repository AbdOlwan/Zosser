using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.PeopleBusServices
{
    public interface ICustomerServices
    {
        Task<List<CustomerDTO>?> GetAllCustomers();
        Task<CustomerDTO?> AddNewCustomer(CustomerDTO customer);

        Task<int> CountCustomers();

        Task<CustomerDTO?> GetCustomerById(int id);

        Task<bool> DeleteCustomerById(int id);

        Task<bool> UpdateCustomerById(CustomerDTO customer);
    }
}
