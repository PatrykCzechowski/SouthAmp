using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SouthAmp.Application.DTOs;
using SouthAmp.Application.Interfaces;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Web.Controllers;
using SouthAmp.Web.Models;
using Xunit;

namespace SouthAmp.Tests
{
    public class ReviewsControllerTests
    {
        private readonly Mock<IReviewUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ReviewsController _controller;
        public ReviewsControllerTests()
        {
            _controller = new ReviewsController(_useCasesMock.Object, _mapperMock.Object);
        }

        private void SetUser(string userId)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "mock");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        [Fact]
        public async Task AddReview_ReturnsOk_WhenValid()
        {
            SetUser("5");
            var dto = new ReviewDto { HotelId = 2 };
            var review = new Review { HotelId = 2, UserId = 5 };
            var resultReview = new Review { Id = 1, HotelId = 2, UserId = 5 };
            var resultDto = new ReviewDto { Id = 1, HotelId = 2, UserId = 5 };
            _mapperMock.Setup(m => m.Map<Review>(dto)).Returns(review);
            _useCasesMock.Setup(u => u.AddReviewAsync(review)).ReturnsAsync(resultReview);
            _mapperMock.Setup(m => m.Map<ReviewDto>(resultReview)).Returns(resultDto);
            var result = await _controller.AddReview(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ReviewDto>>(ok.Value);
            Assert.Equal(resultDto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task AddReview_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var result = await _controller.AddReview(new ReviewDto());
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateReview_ReturnsOk()
        {
            SetUser("7");
            var dto = new ReviewDto { Id = 1, HotelId = 2 };
            var review = new Review { Id = 1, HotelId = 2, UserId = 7 };
            _mapperMock.Setup(m => m.Map<Review>(dto)).Returns(review);
            _useCasesMock.Setup(u => u.UpdateReviewAsync(review)).Returns(Task.CompletedTask);
            var result = await _controller.UpdateReview(1, dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Review updated", apiResponse.Message);
        }

        [Fact]
        public async Task UpdateReview_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var result = await _controller.UpdateReview(1, new ReviewDto());
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeleteReview_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.DeleteReviewAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteReview(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Review deleted", apiResponse.Message);
        }

        [Fact]
        public async Task GetReviewsByHotelId_ReturnsOk()
        {
            var reviews = new List<Review> { new Review { Id = 1, HotelId = 2 } };
            var dtos = new List<ReviewDto> { new ReviewDto { Id = 1, HotelId = 2 } };
            _useCasesMock.Setup(u => u.GetReviewsByHotelIdAsync(2)).ReturnsAsync(reviews);
            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews)).Returns(dtos);
            var result = await _controller.GetReviewsByHotelId(2);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ReviewDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task GetReviewById_ReturnsOk_WhenFound()
        {
            var review = new Review { Id = 1 };
            var dto = new ReviewDto { Id = 1 };
            _useCasesMock.Setup(u => u.GetReviewByIdAsync(1)).ReturnsAsync(review);
            _mapperMock.Setup(m => m.Map<ReviewDto>(review)).Returns(dto);
            var result = await _controller.GetReviewById(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ReviewDto>>(ok.Value);
            Assert.Equal(dto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task GetReviewById_ReturnsNotFound_WhenNull()
        {
            _useCasesMock.Setup(u => u.GetReviewByIdAsync(1)).ReturnsAsync((Review)null);
            var result = await _controller.GetReviewById(1);
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFound.Value);
            Assert.Equal("Review not found", apiResponse.Message);
        }

        [Fact]
        public async Task GetAverageRating_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.GetAverageRatingAsync(2)).ReturnsAsync(4.5);
            var result = await _controller.GetAverageRating(2);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<double>>(ok.Value);
            Assert.Equal(4.5, apiResponse.Data);
        }
    }
}
