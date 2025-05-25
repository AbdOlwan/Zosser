using DAL_OnlineStore.Entities.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using DAL_OnlineStore.Repositories.Interfaces.PeopleRepository;
using BLL_OnlineStore.Interfaces.PeopleBusServices;

namespace BLL_OnlineStore.Services.PeopleBusServices
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepo _repo;
        private readonly IMapper _mapper;

        public CustomerServices(ICustomerRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }

        public async Task<List<CustomerDTO>?> GetAllCustomers()
        {
            var Customers = await _repo.getAllCustomers();
            if (Customers == null)
                return null;

            return _mapper.Map<List<CustomerDTO>>(Customers);

        }

        public async Task<CustomerDTO?> GetCustomerById(int ID)
        {
            var Customer = await _repo.getCustomerById(ID);
            if (Customer == null) return null;

            return _mapper.Map<CustomerDTO>(Customer);
        }

        public async Task<CustomerDTO?> AddNewCustomer(CustomerDTO CustomerDTO)
        {
            var Customer = _mapper.Map<Customer>(CustomerDTO);

            var Newcustomer = await _repo.addNewCustomer(Customer);
            if (Newcustomer != null)
            {
                return _mapper.Map<CustomerDTO?>(Newcustomer);
            }
            return null;
        }
        public async Task<bool> UpdateCustomerById(CustomerDTO CustomerDTO)
        {
            if (CustomerDTO == null)
                return false;

            var Customer = _mapper.Map<Customer>(CustomerDTO);
            return await _repo.updateCustomerById(Customer);
        }
        public async Task<bool> DeleteCustomerById(int id)
        {
            return await _repo.deleteCustomerById(id);
        }

        public async Task<int> CountCustomers()
        {
            return await _repo.countCustomers();
        }
    }
}
