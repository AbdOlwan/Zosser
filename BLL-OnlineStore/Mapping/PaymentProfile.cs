using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Payment_F;
using DAL_OnlineStore.Entities.Models.PaymentModels;

namespace BLL_OnlineStore.Mapping
{
    //PaymentDTO
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentDTO, Payment>();
            CreateMap<Payment, PaymentDTO>();
        }
    }
}
