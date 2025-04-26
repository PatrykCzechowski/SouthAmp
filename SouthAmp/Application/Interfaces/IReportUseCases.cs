using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface IReportUseCases
    {
        Task<Report> AddReportAsync(Report report);
        Task<IEnumerable<Report>> GetUserReportsAsync(int userId);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task RespondToReportAsync(int reportId, string response);
    }
}
