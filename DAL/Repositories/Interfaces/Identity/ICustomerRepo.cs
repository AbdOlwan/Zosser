using DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.Identity
{
    public interface ICustomerRepo
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer?> GetCustomerById(int id);
        Task<Customer?> GetCustomerByUserId(string userId);
        Task<Customer> AddNewCustomer(Customer customer);
        Task<bool> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomerById(int id);
        Task<Customer?> GetCustomerWithDetailsAsync(int customerId);



        /// <summary>
        /// Check if customer exists by ID
        /// </summary>
        Task<bool> CustomerExistsAsync(int customerId);

        /// <summary>
        /// Check if customer exists by User ID
        /// </summary>
        Task<bool> CustomerExistsByUserIdAsync(string userId);

        /// <summary>
        /// Check if customer exists by email
        /// </summary>
        Task<bool> CustomerExistsByEmailAsync(string email);

        /// <summary>
        /// Check if customer exists by phone number
        /// </summary>
        Task<bool> CustomerExistsByPhoneAsync(string phoneNumber);

    }
}
