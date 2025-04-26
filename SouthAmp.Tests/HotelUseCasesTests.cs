using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.Tests
{
    public class HotelUseCasesTests
    {
        [Fact]
        public async Task AddHotelAsync_ShouldReturnHotel()
        {
            // Arrange
            var mockRepo = new Mock<IHotelRepository>();
            var useCases = new HotelUseCases(mockRepo.Object);
            var hotel = new Hotel { Id = 1, Name = "Test Hotel" };
            // Act
            var result = await useCases.AddHotelAsync(hotel);
            // Assert
            result.Should().Be(hotel);
            mockRepo.Verify(r => r.AddAsync(hotel), Times.Once);
        }

        [Fact]
        public async Task GetAllHotelsAsync_ShouldReturnHotels()
        {
            // Arrange
            var hotels = new List<Hotel> { new Hotel { Id = 1, Name = "Hotel1" } };
            var mockRepo = new Mock<IHotelRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(hotels);
            var useCases = new HotelUseCases(mockRepo.Object);
            // Act
            var result = await useCases.GetAllHotelsAsync();
            // Assert
            result.Should().BeEquivalentTo(hotels);
        }
    }
}