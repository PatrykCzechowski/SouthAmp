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

namespace SouthAmp.Tests
{
    public class DiscountCodesControllerTests
    {
        private readonly Mock<IDiscountCodeUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly DiscountCodesController _controller;
        public DiscountCodesControllerTests()
        {
            _controller = new DiscountCodesController(_useCasesMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateDiscountCode_ReturnsOk()
        {
            var dto = new DiscountCodeDto { Code = "ABC" };
            var code = new DiscountCode { Code = "ABC" };
            var resultCode = new DiscountCode { Id = 1, Code = "ABC" };
            var resultDto = new DiscountCodeDto { Id = 1, Code = "ABC" };
            _mapperMock.Setup(m => m.Map<DiscountCode>(dto)).Returns(code);
            _useCasesMock.Setup(u => u.CreateDiscountCodeAsync(code)).ReturnsAsync(resultCode);
            _mapperMock.Setup(m => m.Map<DiscountCodeDto>(resultCode)).Returns(resultDto);
            var result = await _controller.CreateDiscountCode(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<DiscountCodeDto>>(ok.Value);
            Assert.Equal(resultDto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task VerifyCode_ReturnsOk_WhenFound()
        {
            var code = new DiscountCode { Id = 1, Code = "ABC" };
            var dto = new DiscountCodeDto { Id = 1, Code = "ABC" };
            _useCasesMock.Setup(u => u.GetByCodeAsync("ABC")).ReturnsAsync(code);
            _mapperMock.Setup(m => m.Map<DiscountCodeDto>(code)).Returns(dto);
            var result = await _controller.VerifyCode("ABC");
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<DiscountCodeDto>>(ok.Value);
            Assert.Equal(dto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task VerifyCode_ReturnsNotFound_WhenNull()
        {
            _useCasesMock.Setup(u => u.GetByCodeAsync("ABC")).ReturnsAsync((DiscountCode)null);
            var result = await _controller.VerifyCode("ABC");
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFound.Value);
            Assert.Equal("Code not found", apiResponse.Message);
        }

        [Fact]
        public async Task UseCode_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.UseCodeAsync("ABC")).Returns(Task.CompletedTask);
            var result = await _controller.UseCode("ABC");
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Code used", apiResponse.Message);
        }
    }
}
