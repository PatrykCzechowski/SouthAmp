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
    public class NotificationsControllerTests
    {
        private readonly Mock<INotificationUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly NotificationsController _controller;
        public NotificationsControllerTests()
        {
            _controller = new NotificationsController(_useCasesMock.Object, _mapperMock.Object);
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
        public async Task GetMyNotifications_ReturnsOk()
        {
            SetUser("7");
            var notifications = new List<Notification> { new Notification { Id = 1, UserId = 7 } };
            var dtos = new List<NotificationDto> { new NotificationDto { Id = 1, UserId = 7 } };
            _useCasesMock.Setup(u => u.GetUserNotificationsAsync(7)).ReturnsAsync(notifications);
            _mapperMock.Setup(m => m.Map<IEnumerable<NotificationDto>>(notifications)).Returns(dtos);
            var result = await _controller.GetMyNotifications();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<NotificationDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task GetMyNotifications_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var result = await _controller.GetMyNotifications();
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task MarkAsRead_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.MarkAsReadAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.MarkAsRead(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Notification marked as read", apiResponse.Message);
        }

        [Fact]
        public async Task SendNotification_ReturnsOk()
        {
            var dto = new NotificationDto { Message = "test" };
            var notification = new Notification { Message = "test" };
            var resultNotification = new Notification { Id = 1, Message = "test" };
            var resultDto = new NotificationDto { Id = 1, Message = "test" };
            _mapperMock.Setup(m => m.Map<Notification>(dto)).Returns(notification);
            _useCasesMock.Setup(u => u.AddNotificationAsync(notification)).ReturnsAsync(resultNotification);
            _mapperMock.Setup(m => m.Map<NotificationDto>(resultNotification)).Returns(resultDto);
            var result = await _controller.SendNotification(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<NotificationDto>>(ok.Value);
            Assert.Equal(resultDto.Id, apiResponse.Data.Id);
        }
    }
}
