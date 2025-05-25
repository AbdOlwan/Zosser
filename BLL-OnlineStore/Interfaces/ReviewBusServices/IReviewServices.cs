using BLL_OnlineStore.DTOs.EntitiesDTOs.Review_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.ReviewBusServices
{
    public interface IReviewServices
    {
        Task<List<ReviewDTO>?> GetAllReviews();
        Task<ReviewDTO?> AddNewReview(ReviewDTO review);

        Task<ReviewDTO?> GetReviewById(int id);

        Task<bool> DeleteReviewById(int id);

        Task<bool> UpdateReviewById(ReviewDTO review);
    }
}
