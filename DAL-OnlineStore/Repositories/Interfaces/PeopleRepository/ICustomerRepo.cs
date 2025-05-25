using DAL_OnlineStore.Entities.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.PeopleRepository
{
    public interface ICustomerRepo
    {
        Task<List<Customer>?> getAllCustomers();
        Task<Customer> addNewCustomer(Customer customer);

        Task<int> countCustomers();

        Task<Customer?> getCustomerById(int id);

        Task<bool> deleteCustomerById(int id);

        Task<bool> updateCustomerById(Customer customer);

    }
}
