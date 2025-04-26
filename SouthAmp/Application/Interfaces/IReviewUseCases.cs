using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface IReviewUseCases
    {
        Task<Review> AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByHotelIdAsync(int hotelId);
        Task<Review> GetReviewByIdAsync(int id);
        Task<double> GetAverageRatingAsync(int hotelId);
    }
}
