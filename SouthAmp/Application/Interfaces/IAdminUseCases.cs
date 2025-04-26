using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface IAdminUseCases
    {
        Task<IEnumerable<AppUserProfile>> GetAllUsersAsync();
        Task BanUserAsync(int userId);
        Task ActivateUserAsync(int userId);
        Task DeleteUserAsync(int userId);
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task ModerateHotelAsync(int hotelId, bool isActive);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task ModerateReviewAsync(int reviewId, bool isReported);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
    }
}
