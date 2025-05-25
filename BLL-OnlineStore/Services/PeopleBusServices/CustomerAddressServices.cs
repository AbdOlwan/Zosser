using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using BLL_OnlineStore.Interfaces.PeopleBusServices;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Repositories.Interfaces.PeopleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services.PeopleBusServices
{
    public class CustomerAddressServices : ICustomerAddressServices
    {
        private readonly ICustomerAddressRepo _repo;
        private readonly IMapper _mapper;

        public CustomerAddressServices(ICustomerAddressRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CustomerAddressDTO>?> GetAllCustomerAddresses()
        {
            var CustomerAddresses = await _repo.getAllCustomerAddresss();
            if (CustomerAddresses == null)
                return null;

            return _mapper.Map<List<CustomerAddressDTO>>(CustomerAddresses);



        }

        public async Task<CustomerAddressDTO?> GetCustomerAddressById(int AddressID)
        {
            var CustomerAddress = await _repo.getCustomerAddressById(AddressID);
            if (CustomerAddress == null) return null;
            return _mapper?.Map<CustomerAddressDTO>(CustomerAddress);

        }

        public async Task<CustomerAddressDTO?> AddNewCustomerAddress(CustomerAddressDTO customerAddressDTO)
        {
            var CustomerAddress = _mapper.Map<CustomerAddress>(customerAddressDTO);



            var NewcustomerAddress = await _repo.addNewCustomerAddress(CustomerAddress);
            if (NewcustomerAddress != null)
            {
                return _mapper.Map<CustomerAddressDTO>(NewcustomerAddress);

            }
            return null;
        }
        public async Task<bool> UpdateCustomerAddressById(CustomerAddressDTO CustomerAddressDTO)
        {
            if (CustomerAddressDTO == null)
                return false;

            var CustomerAddress = _mapper.Map<CustomerAddress>(CustomerAddressDTO);

            return await _repo.updateCustomerAddressById(CustomerAddress);
        }
        public async Task<bool> DeleteCustomerAddressById(int id)
        {
            return await _repo.deleteCustomerAddressById(id);
        }
    }
}
