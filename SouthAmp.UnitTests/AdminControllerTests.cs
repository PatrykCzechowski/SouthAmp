using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SouthAmp.Application.DTOs;
using SouthAmp.Application.Interfaces;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Infrastructure.Identity;
using SouthAmp.Web.Controllers;
using SouthAmp.Web.Models;
using Xunit;

namespace SouthAmp.UnitTests
{
    public class AdminControllerTests
    {
        private readonly Mock<IAdminUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly AdminController _controller;
        public AdminControllerTests()
        {
            _controller = new(_useCasesMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOk()
        {
            var users = new List<AppUser> { new() };
            var dtos = new List<UserDto> { new() };
            _useCasesMock.Setup(u => u.GetAllUsersAsync()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(dtos);
            var result = await _controller.GetAllUsers();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<UserDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task ActivateUser_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.ActivateUserAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.ActivateUser(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("User activated", apiResponse.Message);
        }

        [Fact]
        public async Task BanUser_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.BanUserAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.BanUser(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("User banned", apiResponse.Message);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.DeleteUserAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteUser(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("User deleted", apiResponse.Message);
        }

        [Fact]
        public async Task GetAllHotels_ReturnsOk()
        {
            var hotels = new List<Hotel> { new() };
            var dtos = new List<HotelDto> { new() };
            _useCasesMock.Setup(u => u.GetAllHotelsAsync()).ReturnsAsync(hotels);
            _mapperMock.Setup(m => m.Map<IEnumerable<HotelDto>>(hotels)).Returns(dtos);
            var result = await _controller.GetAllHotels();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<HotelDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task ModerateHotel_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.ModerateHotelAsync(1, true)).Returns(Task.CompletedTask);
            var result = await _controller.ModerateHotel(1, true);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Hotel moderated", apiResponse.Message);
        }

        [Fact]
        public async Task GetAllReviews_ReturnsOk()
        {
            var reviews = new List<Review> { new() };
            var dtos = new List<ReviewDto> { new() };
            _useCasesMock.Setup(u => u.GetAllReviewsAsync()).ReturnsAsync(reviews);
            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews)).Returns(dtos);
            var result = await _controller.GetAllReviews();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ReviewDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task ModerateReview_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.ModerateReviewAsync(1, true)).Returns(Task.CompletedTask);
            var result = await _controller.ModerateReview(1, true);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Review moderated", apiResponse.Message);
        }

        [Fact]
        public async Task GetAllPayments_ReturnsOk()
        {
            var payments = new List<Payment> { new() };
            var dtos = new List<PaymentDto> { new() };
            _useCasesMock.Setup(u => u.GetAllPaymentsAsync()).ReturnsAsync(payments);
            _mapperMock.Setup(m => m.Map<IEnumerable<PaymentDto>>(payments)).Returns(dtos);
            var result = await _controller.GetAllPayments();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<PaymentDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }
    }
}
