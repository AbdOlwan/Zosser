using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Review_F;
using DAL_OnlineStore.Entities.Models.ReviewModels;

namespace BLL_OnlineStore.Mapping
{
    //Review

    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ReviewDTO, Review>();
            CreateMap<Review, ReviewDTO>();
        }
    }
}
