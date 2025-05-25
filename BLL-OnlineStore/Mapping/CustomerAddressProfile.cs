using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using DAL_OnlineStore.Entities.Models.People;

namespace BLL_OnlineStore.Mapping
{
    //CustomerAddressDTO

    public class CustomerAddressProfile : Profile
    {
        public CustomerAddressProfile()
        {
            CreateMap<CustomerAddressDTO, CustomerAddress>();
            CreateMap<CustomerAddress, CustomerAddressDTO>();
        }
    }
}
