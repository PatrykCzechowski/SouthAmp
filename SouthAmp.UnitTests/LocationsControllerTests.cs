using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SouthAmp.Application.DTOs;
using SouthAmp.Application.Interfaces;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Web.Controllers;
using SouthAmp.Web.Models;
using Xunit;

namespace SouthAmp.UnitTests
{
    public class LocationsControllerTests
    {
        private readonly Mock<ILocationUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly LocationsController _controller;
        public LocationsControllerTests()
        {
            _controller = new LocationsController(_useCasesMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddLocation_ReturnsOk()
        {
            var dto = new LocationDto { City = "TestCity" };
            var location = new Location { City = "TestCity" };
            var resultLocation = new Location { Id = 1, City = "TestCity" };
            var resultDto = new LocationDto { Id = 1, City = "TestCity" };
            _mapperMock.Setup(m => m.Map<Location>(dto)).Returns(location);
            _useCasesMock.Setup(u => u.AddLocationAsync(location)).ReturnsAsync(resultLocation);
            _mapperMock.Setup(m => m.Map<LocationDto>(resultLocation)).Returns(resultDto);
            var result = await _controller.AddLocation(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<LocationDto>>(ok.Value);
            Assert.Equal(resultDto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task GetAllLocations_ReturnsOk()
        {
            var locations = new List<Location> { new Location { Id = 1 } };
            var dtos = new List<LocationDto> { new LocationDto { Id = 1 } };
            _useCasesMock.Setup(u => u.GetAllLocationsAsync()).ReturnsAsync(locations);
            _mapperMock.Setup(m => m.Map<IEnumerable<LocationDto>>(locations)).Returns(dtos);
            var result = await _controller.GetAllLocations();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<LocationDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task GetLocationById_ReturnsOk_WhenFound()
        {
            var location = new Location { Id = 1 };
            var dto = new LocationDto { Id = 1 };
            _useCasesMock.Setup(u => u.GetLocationByIdAsync(1)).ReturnsAsync(location);
            _mapperMock.Setup(m => m.Map<LocationDto>(location)).Returns(dto);
            var result = await _controller.GetLocationById(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<LocationDto>>(ok.Value);
            Assert.Equal(dto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task GetLocationById_ReturnsNotFound_WhenNull()
        {
            _useCasesMock.Setup(u => u.GetLocationByIdAsync(1))!.ReturnsAsync((Location)null);
            var result = await _controller.GetLocationById(1);
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFound.Value);
            Assert.Equal("Location not found", apiResponse.Message);
        }

        [Fact]
        public async Task UpdateLocation_ReturnsOk()
        {
            var dto = new LocationDto { Id = 1, City = "TestCity" };
            var location = new Location { Id = 1, City = "TestCity" };
            _mapperMock.Setup(m => m.Map<Location>(dto)).Returns(location);
            _useCasesMock.Setup(u => u.UpdateLocationAsync(location)).Returns(Task.CompletedTask);
            var result = await _controller.UpdateLocation(1, dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Location updated", apiResponse.Message);
        }

        [Fact]
        public async Task DeleteLocation_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.DeleteLocationAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteLocation(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Location deleted", apiResponse.Message);
        }
    }
}
