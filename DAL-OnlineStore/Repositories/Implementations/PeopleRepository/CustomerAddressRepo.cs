using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces.PeopleRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.PeopleRepository
{
    public class CustomerAddressRepo : ICustomerAddressRepo
    {
        private readonly AppDbContext _context;

        public CustomerAddressRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerAddress>?> getAllCustomerAddresss()
        {
            return await _context.CustomerAddresses
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<CustomerAddress?> getCustomerAddressById(int Id)
        {
            return await _context.CustomerAddresses.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.AddressID == Id);
        }

        public async Task<CustomerAddress> addNewCustomerAddress(CustomerAddress customerAddress)
        {
            await _context.CustomerAddresses.AddAsync(customerAddress);
            await _context.SaveChangesAsync();
            return customerAddress;
        }
        public async Task<bool> deleteCustomerAddressById(int id)
        {
            var CustomerAddress = await _context.CustomerAddresses.FirstOrDefaultAsync(d => d.AddressID == id);
            if (CustomerAddress == null)
            {
                return false;
            }

            _context.CustomerAddresses.Remove(CustomerAddress);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> updateCustomerAddressById(CustomerAddress customerAddress)
        {
            var result = await _context.CustomerAddresses.FirstOrDefaultAsync(d => d.AddressID == customerAddress.AddressID);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(customerAddress);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
