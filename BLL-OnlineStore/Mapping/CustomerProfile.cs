using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using DAL_OnlineStore.Entities.Models.People;

namespace BLL_OnlineStore.Mapping
{
    //CustomerDTO

    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerDTO, Customer>();
            CreateMap<Customer, CustomerDTO>();
        }
    }
}
