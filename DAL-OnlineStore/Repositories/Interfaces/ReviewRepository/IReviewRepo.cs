using DAL_OnlineStore.Entities.Models.PaymentModels;
using DAL_OnlineStore.Entities.Models.ReviewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.ReviewRepository
{
    public interface IReviewRepo
    {
        Task<List<Review>?> getAllReviews();
        Task<Review> addNewReview(Review review);

        //Task<int> countCustomers();

        Task<Review?> getReviewById(int id);

        Task<bool> deleteReviewById(int id);

        Task<bool> updateReviewById(Review review);
    }
}
