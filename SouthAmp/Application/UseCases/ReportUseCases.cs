using SouthAmp.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class ReportUseCases : IReportUseCases
    {
        private readonly IReportRepository _reportRepository;
        public ReportUseCases(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<Report> AddReportAsync(Report report)
        {
            report.CreatedAt = System.DateTime.UtcNow;
            report.Status = ReportStatus.Open;
            await _reportRepository.AddAsync(report);
            return report;
        }
        public async Task<IEnumerable<Report>> GetUserReportsAsync(int userId)
        {
            return await _reportRepository.GetByUserIdAsync(userId);
        }
        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            // For admin
            return await _reportRepository.GetByUserIdAsync(0); // 0 = all
        }
        public async Task RespondToReportAsync(int reportId, string response)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report != null)
            {
                report.Response = response;
                report.RespondedAt = System.DateTime.UtcNow;
                report.Status = ReportStatus.Closed;
                await _reportRepository.UpdateAsync(report);
            }
        }
    }
}