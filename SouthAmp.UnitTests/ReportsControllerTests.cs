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

namespace SouthAmp.UnitTests
{
    public class ReportsControllerTests
    {
        private readonly Mock<IReportUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ReportsController _controller;
        public ReportsControllerTests()
        {
            _controller = new ReportsController(_useCasesMock.Object, _mapperMock.Object);
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
        public async Task AddReport_ReturnsOk_WhenValid()
        {
            SetUser("5");
            var dto = new ReportDto { Message = "test message" };
            var report = new Report { Message = "test message", UserId = 5 };
            var resultReport = new Report { Id = 1, Message = "test message", UserId = 5 };
            var resultDto = new ReportDto { Id = 1, Message = "test message", UserId = 5 };
            _mapperMock.Setup(m => m.Map<Report>(dto)).Returns(report);
            _useCasesMock.Setup(u => u.AddReportAsync(report)).ReturnsAsync(resultReport);
            _mapperMock.Setup(m => m.Map<ReportDto>(resultReport)).Returns(resultDto);
            var result = await _controller.AddReport(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ReportDto>>(ok.Value);
            Assert.Equal(resultDto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task AddReport_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var result = await _controller.AddReport(new ReportDto());
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetMyReports_ReturnsOk()
        {
            SetUser("7");
            var reports = new List<Report> { new Report { Id = 1, UserId = 7 } };
            var dtos = new List<ReportDto> { new ReportDto { Id = 1, UserId = 7 } };
            _useCasesMock.Setup(u => u.GetUserReportsAsync(7)).ReturnsAsync(reports);
            _mapperMock.Setup(m => m.Map<IEnumerable<ReportDto>>(reports)).Returns(dtos);
            var result = await _controller.GetMyReports();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ReportDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task GetMyReports_ReturnsUnauthorized_WhenNoUser()
        {
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var result = await _controller.GetMyReports();
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetAllReports_ReturnsOk()
        {
            var reports = new List<Report> { new Report { Id = 1 } };
            var dtos = new List<ReportDto> { new ReportDto { Id = 1 } };
            _useCasesMock.Setup(u => u.GetAllReportsAsync()).ReturnsAsync(reports);
            _mapperMock.Setup(m => m.Map<IEnumerable<ReportDto>>(reports)).Returns(dtos);
            var result = await _controller.GetAllReports();
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<ReportDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task RespondToReport_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.RespondToReportAsync(1, "response")).Returns(Task.CompletedTask);
            var req = new ReportsController.RespondRequest { Response = "response" };
            var result = await _controller.RespondToReport(1, req);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Report responded", apiResponse.Message);
        }
    }
}
