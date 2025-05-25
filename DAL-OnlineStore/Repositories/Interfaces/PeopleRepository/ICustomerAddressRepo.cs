using DAL_OnlineStore.Entities.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.PeopleRepository
{
    public interface ICustomerAddressRepo
    {
        Task<List<CustomerAddress>?> getAllCustomerAddresss();
        Task<CustomerAddress> addNewCustomerAddress(CustomerAddress customerAddress);

        //Task<int> countCustomers();

        Task<CustomerAddress?> getCustomerAddressById(int id);

        Task<bool> deleteCustomerAddressById(int id);

        Task<bool> updateCustomerAddressById(CustomerAddress customerAddress);
    }
}
