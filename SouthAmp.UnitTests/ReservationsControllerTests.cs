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
using SouthAmp.Infrastructure.Services;
using SouthAmp.Web.Controllers;
using SouthAmp.Web.Models;
using Xunit;

namespace SouthAmp.UnitTests
{
    public class ReservationsControllerTests
    {
        private readonly Mock<IReservationUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IAuditService> _auditMock = new();
        private readonly Mock<IEmailService> _emailMock = new();
        private readonly ReservationsController _controller;
        public ReservationsControllerTests()
        {
            _controller = new ReservationsController(
                _useCasesMock.Object,
                _mapperMock.Object,
                _auditMock.Object,
                _emailMock.Object
            );
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
        public async Task CreateReservation_ReturnsOk_WhenValid()
        {
            SetUser("5");
            var dto = new ReservationDto { RoomId = 2 };
            var reservation = new Reservation { RoomId = 2, UserId = 5 };
            var resultReservation = new Reservation { Id = 1, RoomId = 2, UserId = 5 };
            var resultDto = new ReservationDto { Id = 1, RoomId = 2, UserId = 5 };
            _mapperMock.Setup(m => m.Map<Reservation>(dto)).Returns(reservation);
            _useCasesMock.Setup(u => u.CreateReservationAsync(reservation)).ReturnsAsync(resultReservation);
            _mapperMock.Setup(m => m.Map<ReservationDto>(resultReservation)).Returns(resultDto);
            var result = await _controller.CreateReservation(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ReservationDto>>(ok.Value);
            Assert.Equal(resultDto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task CreateReservation_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var result = await _controller.CreateReservation(new ReservationDto());
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetMyReservations_ReturnsOk()
        {
            SetUser("7");
            var reservations = new List<Reservation> { new Reservation { Id = 1, UserId = 7 } };
            var dtos = new List<ReservationDto> { new ReservationDto { Id = 1, UserId = 7 } };
            _useCasesMock.Setup(u => u.GetUserReservationsAsync(7)).ReturnsAsync(reservations);
            _mapperMock.Setup(m => m.Map<IEnumerable<ReservationDto>>(reservations)).Returns(dtos);
            var result = await _controller.GetMyReservations();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ReservationDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task GetMyReservations_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var result = await _controller.GetMyReservations();
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CheckAvailability_ReturnsOk()
        {
            var req = new ReservationsController.CheckAvailabilityRequest { RoomId = 1, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1) };
            _useCasesMock.Setup(u => u.CheckAvailabilityAsync(1, req.StartDate, req.EndDate)).ReturnsAsync(true);
            var result = await _controller.CheckAvailability(req);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<bool>>(ok.Value);
            Assert.True(apiResponse.Data);
        }

        [Fact]
        public async Task CancelReservation_ReturnsOk()
        {
            SetUser("1");
            _useCasesMock.Setup(u => u.CancelReservationAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.CancelReservation(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Reservation cancelled", apiResponse.Message);
        }

        [Fact]
        public async Task ChangeReservationDate_ReturnsOk()
        {
            SetUser("1");
            var req = new ReservationsController.ChangeDateRequest { NewStart = DateTime.Today, NewEnd = DateTime.Today.AddDays(2) };
            _useCasesMock.Setup(u => u.ChangeReservationDateAsync(1, req.NewStart, req.NewEnd)).Returns(Task.CompletedTask);
            var result = await _controller.ChangeReservationDate(1, req);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Reservation date changed", apiResponse.Message);
        }
    }
}
