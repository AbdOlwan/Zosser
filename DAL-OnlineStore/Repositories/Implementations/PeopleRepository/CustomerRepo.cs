using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces.PeopleRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.PeopleRepository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext _context;

        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>?> getAllCustomers()
        {
            return await _context.Customers
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<Customer?> getCustomerById(int Id)
        {
            return await _context.Customers.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.CustomerId == Id);
        }

        public async Task<Customer> addNewCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<bool> deleteCustomerById(int id)
        {
            var Customer = await _context.Customers.FirstOrDefaultAsync(d => d.CustomerId == id);
            if (Customer == null)
            {
                return false;
            }

            _context.Customers.Remove(Customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateCustomerById(Customer customer)
        {
            var result = await _context.Customers.FirstOrDefaultAsync(d => d.CustomerId == customer.CustomerId);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(customer);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> countCustomers()
        {
            return await _context.Customers.CountAsync();
        }
    }
}
