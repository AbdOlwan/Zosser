using DAL_OnlineStore.Entities.Models.ReviewModels;
using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Review_F;
using DAL_OnlineStore.Repositories.Interfaces.ReviewRepository;
using BLL_OnlineStore.Interfaces.ReviewBusServices;

namespace BLL_OnlineStore.Services.ReviewBusServices
{
    public class ReviewServices : IReviewServices
    {
        private readonly IReviewRepo _repo;
        private readonly IMapper _mapper;

        public ReviewServices(IReviewRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ReviewDTO>?> GetAllReviews()
        {
            var Reviews = await _repo.getAllReviews();
            if (Reviews == null)
                return null;

            return _mapper.Map<List<ReviewDTO>>(Reviews);

        }

        public async Task<ReviewDTO?> GetReviewById(int ID)
        {
            var Review = await _repo.getReviewById(ID);
            if (Review == null) return null;

            return _mapper?.Map<ReviewDTO>(Review);
        }

        public async Task<ReviewDTO?> AddNewReview(ReviewDTO DTO)
        {
            var Review = _mapper.Map<Review>(DTO);

            var NewReview = await _repo.addNewReview(Review);
            if (NewReview != null)
            {
                return _mapper.Map<ReviewDTO>(NewReview);
            }
            return null;
        }
        public async Task<bool> UpdateReviewById(ReviewDTO DTO)
        {
            if (DTO == null)
                return false;

            var Review = _mapper.Map<Review>(DTO);
            return await _repo.updateReviewById(Review);
        }
        public async Task<bool> DeleteReviewById(int id)
        {
            return await _repo.deleteReviewById(id);
        }

    }
}
