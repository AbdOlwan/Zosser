using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Entities.Models.ReviewModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces.ReviewRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.ReviewRepository
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext _context;

        public ReviewRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>?> getAllReviews()
        {
            return await _context.Reviews
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<Review?> getReviewById(int Id)
        {
            return await _context.Reviews.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.ReviewID == Id);
        }

        public async Task<Review> addNewReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }
        public async Task<bool> deleteReviewById(int id)
        {
            var Review = await _context.Reviews.FirstOrDefaultAsync(d => d.ReviewID == id);
            if (Review == null)
            {
                return false;
            }

            _context.Reviews.Remove(Review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateReviewById(Review review)
        {
            var result = await _context.Reviews.FirstOrDefaultAsync(d => d.ReviewID == review.ReviewID);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(review);


                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
