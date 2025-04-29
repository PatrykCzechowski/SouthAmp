using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Application.Interfaces;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using SouthAmp.Infrastructure.Identity;

namespace SouthAmp.Application.UseCases
{
    public class AdminUseCases : IAdminUseCases
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IPaymentRepository _paymentRepository;
        public AdminUseCases(IUserRepository userRepository, IHotelRepository hotelRepository, IReviewRepository reviewRepository, IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _hotelRepository = hotelRepository;
            _reviewRepository = reviewRepository;
            _paymentRepository = paymentRepository;
        }
        public async Task<IEnumerable<AppUser>> GetAllUsersAsync() => await _userRepository.GetAllAsync();
        public async Task BanUserAsync(int userId) { var user = await _userRepository.GetByIdAsync(userId); if (user != null) { user.IsActive = false; await _userRepository.UpdateAsync(user); } }
        public async Task ActivateUserAsync(int userId) { var user = await _userRepository.GetByIdAsync(userId); if (user != null) { user.IsActive = true; await _userRepository.UpdateAsync(user); } }
        public async Task DeleteUserAsync(int userId) => await _userRepository.DeleteAsync(userId);
        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync() => await _hotelRepository.GetAllAsync();
        public async Task ModerateHotelAsync(int hotelId, bool isActive) { var hotel = await _hotelRepository.GetByIdAsync(hotelId); if (hotel != null) { hotel.IsActive = isActive; await _hotelRepository.UpdateAsync(hotel); } }
        public async Task<IEnumerable<Review>> GetAllReviewsAsync() => await _reviewRepository.GetAllAsync();
        public async Task ModerateReviewAsync(int reviewId, bool isReported) { var review = await _reviewRepository.GetByIdAsync(reviewId); if (review != null) { review.IsReported = isReported; await _reviewRepository.UpdateAsync(review); } }
        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync() => await _paymentRepository.GetAllAsync();
    }
}