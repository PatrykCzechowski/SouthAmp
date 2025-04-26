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
    public class PaymentsControllerTests
    {
        private readonly Mock<IPaymentUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly PaymentsController _controller;
        public PaymentsControllerTests()
        {
            _controller = new PaymentsController(_useCasesMock.Object, _mapperMock.Object);
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
        public async Task CreatePayment_ReturnsOk()
        {
            var dto = new PaymentDto { Amount = 100 };
            var payment = new Payment { Amount = 100 };
            var resultPayment = new Payment { Id = 1, Amount = 100 };
            var resultDto = new PaymentDto { Id = 1, Amount = 100 };
            _mapperMock.Setup(m => m.Map<Payment>(dto)).Returns(payment);
            _useCasesMock.Setup(u => u.CreatePaymentAsync(payment)).ReturnsAsync(resultPayment);
            _mapperMock.Setup(m => m.Map<PaymentDto>(resultPayment)).Returns(resultDto);
            var result = await _controller.CreatePayment(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<PaymentDto>>(ok.Value);
            Assert.Equal(resultDto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task ConfirmPayment_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.ConfirmPaymentAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.ConfirmPayment(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Payment confirmed", apiResponse.Message);
        }

        [Fact]
        public async Task RefundPayment_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.RefundPaymentAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.RefundPayment(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Payment refunded", apiResponse.Message);
        }

        [Fact]
        public async Task GetMyPayments_ReturnsOk()
        {
            SetUser("7");
            var payments = new List<Payment> { new Payment { Id = 1, Amount = 100 } };
            var dtos = new List<PaymentDto> { new PaymentDto { Id = 1, Amount = 100 } };
            _useCasesMock.Setup(u => u.GetUserPaymentsAsync(7)).ReturnsAsync(payments);
            _mapperMock.Setup(m => m.Map<IEnumerable<PaymentDto>>(payments)).Returns(dtos);
            var result = await _controller.GetMyPayments();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<PaymentDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task GetMyPayments_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var result = await _controller.GetMyPayments();
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
