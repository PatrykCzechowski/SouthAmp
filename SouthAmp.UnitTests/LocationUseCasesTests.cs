using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.UnitTests
{
    public class LocationUseCasesTests
    {
        private readonly Mock<ILocationRepository> _locationRepoMock = new();
        private readonly LocationUseCases _sut;

        public LocationUseCasesTests()
        {
            _sut = new LocationUseCases(_locationRepoMock.Object);
        }

        [Fact]
        public async Task AddLocationAsync_AddsLocation()
        {
            var location = new Location { Id = 1 };
            _locationRepoMock.Setup(r => r.AddAsync(location)).Returns(Task.CompletedTask);
            var result = await _sut.AddLocationAsync(location);
            Assert.Equal(location, result);
        }

        [Fact]
        public async Task GetAllLocationsAsync_ReturnsLocations()
        {
            var locations = new List<Location> { new Location { Id = 1 } };
            _locationRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(locations);
            var result = await _sut.GetAllLocationsAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task GetLocationByIdAsync_ReturnsLocation()
        {
            var location = new Location { Id = 1 };
            _locationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);
            var result = await _sut.GetLocationByIdAsync(1);
            Assert.Equal(location, result);
        }

        [Fact]
        public async Task UpdateLocationAsync_UpdatesLocation()
        {
            var location = new Location { Id = 1 };
            _locationRepoMock.Setup(r => r.UpdateAsync(location)).Returns(Task.CompletedTask);
            await _sut.UpdateLocationAsync(location);
            _locationRepoMock.Verify(r => r.UpdateAsync(location), Times.Once);
        }

        [Fact]
        public async Task DeleteLocationAsync_DeletesLocation()
        {
            _locationRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
            await _sut.DeleteLocationAsync(1);
            _locationRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
