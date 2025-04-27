using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.UnitTests
{
    public class AdminUseCasesTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IHotelRepository> _hotelRepoMock = new();
        private readonly Mock<IReviewRepository> _reviewRepoMock = new();
        private readonly Mock<IPaymentRepository> _paymentRepoMock = new();
        private readonly AdminUseCases _sut;

        public AdminUseCasesTests()
        {
            _sut = new AdminUseCases(_userRepoMock.Object, _hotelRepoMock.Object, _reviewRepoMock.Object, _paymentRepoMock.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsUsers()
        {
            var users = new List<AppUserProfile> { new AppUserProfile() };
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            var result = await _sut.GetAllUsersAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task BanUserAsync_UpdatesUser_WhenUserExists()
        {
            var user = new AppUserProfile { Id = 1, IsActive = true };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);
            await _sut.BanUserAsync(1);
            Assert.False(user.IsActive);
        }

        [Fact]
        public async Task BanUserAsync_DoesNothing_WhenUserNull()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((AppUserProfile)null);
            await _sut.BanUserAsync(1);
            _userRepoMock.Verify(r => r.UpdateAsync(It.IsAny<AppUserProfile>()), Times.Never);
        }

        [Fact]
        public async Task ActivateUserAsync_UpdatesUser_WhenUserExists()
        {
            var user = new AppUserProfile { Id = 1, IsActive = false };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);
            await _sut.ActivateUserAsync(1);
            Assert.True(user.IsActive);
        }

        [Fact]
        public async Task ActivateUserAsync_DoesNothing_WhenUserNull()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((AppUserProfile)null);
            await _sut.ActivateUserAsync(1);
            _userRepoMock.Verify(r => r.UpdateAsync(It.IsAny<AppUserProfile>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            _userRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
            await _sut.DeleteUserAsync(1);
            _userRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAllHotelsAsync_ReturnsHotels()
        {
            var hotels = new List<Hotel> { new Hotel() };
            _hotelRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(hotels);
            var result = await _sut.GetAllHotelsAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task ModerateHotelAsync_UpdatesHotel_WhenHotelExists()
        {
            var hotel = new Hotel { Id = 1, IsActive = false };
            _hotelRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hotel);
            _hotelRepoMock.Setup(r => r.UpdateAsync(hotel)).Returns(Task.CompletedTask);
            await _sut.ModerateHotelAsync(1, true);
            Assert.True(hotel.IsActive);
        }

        [Fact]
        public async Task ModerateHotelAsync_DoesNothing_WhenHotelNull()
        {
            _hotelRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Hotel)null);
            await _sut.ModerateHotelAsync(1, true);
            _hotelRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
        }

        [Fact]
        public async Task GetAllReviewsAsync_ReturnsReviews()
        {
            var reviews = new List<Review> { new Review() };
            _reviewRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reviews);
            var result = await _sut.GetAllReviewsAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task ModerateReviewAsync_UpdatesReview_WhenReviewExists()
        {
            var review = new Review { Id = 1, IsReported = false };
            _reviewRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(review);
            _reviewRepoMock.Setup(r => r.UpdateAsync(review)).Returns(Task.CompletedTask);
            await _sut.ModerateReviewAsync(1, true);
            Assert.True(review.IsReported);
        }

        [Fact]
        public async Task ModerateReviewAsync_DoesNothing_WhenReviewNull()
        {
            _reviewRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Review)null);
            await _sut.ModerateReviewAsync(1, true);
            _reviewRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Never);
        }

        [Fact]
        public async Task GetAllPaymentsAsync_ReturnsPayments()
        {
            var payments = new List<Payment> { new Payment() };
            _paymentRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(payments);
            var result = await _sut.GetAllPaymentsAsync();
            Assert.Single(result);
        }
    }
}
