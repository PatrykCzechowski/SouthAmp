using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.Tests
{
    public class ReviewUseCasesTests
    {
        private readonly Mock<IReviewRepository> _reviewRepoMock = new();
        private readonly ReviewUseCases _sut;

        public ReviewUseCasesTests()
        {
            _sut = new ReviewUseCases(_reviewRepoMock.Object);
        }

        [Fact]
        public async Task AddReviewAsync_SetsCreatedAtAndAddsReview()
        {
            var review = new Review { Id = 1 };
            _reviewRepoMock.Setup(r => r.AddAsync(review)).Returns(Task.CompletedTask);
            var result = await _sut.AddReviewAsync(review);
            Assert.Equal(review, result);
            Assert.True((DateTime.UtcNow - result.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public async Task UpdateReviewAsync_SetsUpdatedAtAndUpdatesReview()
        {
            var review = new Review { Id = 1 };
            _reviewRepoMock.Setup(r => r.UpdateAsync(review)).Returns(Task.CompletedTask);
            await _sut.UpdateReviewAsync(review);
            Assert.True((DateTime.UtcNow - review.UpdatedAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public async Task DeleteReviewAsync_DeletesReview()
        {
            _reviewRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
            await _sut.DeleteReviewAsync(1);
            _reviewRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetReviewsByHotelIdAsync_ReturnsReviews()
        {
            var reviews = new List<Review> { new Review { Id = 1 } };
            _reviewRepoMock.Setup(r => r.GetByHotelIdAsync(1)).ReturnsAsync(reviews);
            var result = await _sut.GetReviewsByHotelIdAsync(1);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetReviewByIdAsync_ReturnsReview()
        {
            var review = new Review { Id = 1 };
            _reviewRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(review);
            var result = await _sut.GetReviewByIdAsync(1);
            Assert.Equal(review, result);
        }

        [Fact]
        public async Task GetAverageRatingAsync_ReturnsZero_WhenNoReviews()
        {
            _reviewRepoMock.Setup(r => r.GetByHotelIdAsync(1)).ReturnsAsync(new List<Review>());
            var result = await _sut.GetAverageRatingAsync(1);
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetAverageRatingAsync_ReturnsAverage_WhenReviewsExist()
        {
            var reviews = new List<Review> { new Review { Rating = 4 }, new Review { Rating = 2 } };
            _reviewRepoMock.Setup(r => r.GetByHotelIdAsync(1)).ReturnsAsync(reviews);
            var result = await _sut.GetAverageRatingAsync(1);
            Assert.Equal(3, result);
        }
    }
}
