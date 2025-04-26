using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class ReviewUseCases
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewUseCases(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<Review> AddReviewAsync(Review review)
        {
            review.CreatedAt = System.DateTime.UtcNow;
            await _reviewRepository.AddAsync(review);
            return review;
        }
        public async Task UpdateReviewAsync(Review review)
        {
            review.UpdatedAt = System.DateTime.UtcNow;
            await _reviewRepository.UpdateAsync(review);
        }
        public async Task DeleteReviewAsync(int id)
        {
            await _reviewRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId)
        {
            return await _reviewRepository.GetByHotelIdAsync(hotelId);
        }
        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _reviewRepository.GetByIdAsync(id);
        }
        public async Task<double> GetAverageRatingAsync(int hotelId)
        {
            var reviews = await _reviewRepository.GetByHotelIdAsync(hotelId);
            if (!reviews.Any()) return 0;
            return reviews.Average(r => r.Rating);
        }
    }
}