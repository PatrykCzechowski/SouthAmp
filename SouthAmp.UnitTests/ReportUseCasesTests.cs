using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.UnitTests
{
    public class ReportUseCasesTests
    {
        private readonly Mock<IReportRepository> _reportRepoMock = new();
        private readonly ReportUseCases _sut;

        public ReportUseCasesTests()
        {
            _sut = new ReportUseCases(_reportRepoMock.Object);
        }

        [Fact]
        public async Task AddReportAsync_SetsDefaultsAndAdds()
        {
            var report = new Report { Id = 1 };
            _reportRepoMock.Setup(r => r.AddAsync(report)).Returns(Task.CompletedTask);
            var result = await _sut.AddReportAsync(report);
            Assert.Equal(ReportStatus.Open, result.Status);
            Assert.True((DateTime.UtcNow - result.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public async Task GetUserReportsAsync_ReturnsReports()
        {
            var reports = new List<Report> { new Report { Id = 1 } };
            _reportRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(reports);
            var result = await _sut.GetUserReportsAsync(1);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllReportsAsync_ReturnsAllReports()
        {
            var reports = new List<Report> { new Report { Id = 1 } };
            _reportRepoMock.Setup(r => r.GetByUserIdAsync(0)).ReturnsAsync(reports);
            var result = await _sut.GetAllReportsAsync();
            Assert.Single(result);
        }

        [Fact]
        public async Task RespondToReportAsync_UpdatesReport_WhenFound()
        {
            var report = new Report { Id = 1, Status = ReportStatus.Open };
            _reportRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(report);
            _reportRepoMock.Setup(r => r.UpdateAsync(report)).Returns(Task.CompletedTask);
            await _sut.RespondToReportAsync(1, "response");
            Assert.Equal("response", report.Response);
            Assert.Equal(ReportStatus.Closed, report.Status);
            Assert.True((DateTime.UtcNow - report.RespondedAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public async Task RespondToReportAsync_DoesNothing_WhenNotFound()
        {
            _reportRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Report)null);
            await _sut.RespondToReportAsync(1, "response");
            _reportRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Report>()), Times.Never);
        }
    }
}
