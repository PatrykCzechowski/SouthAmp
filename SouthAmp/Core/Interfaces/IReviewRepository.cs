using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId);
        Task<IEnumerable<Review>> GetAllAsync();
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(int id);
        // ...inne metody repozytorium...
    }
}