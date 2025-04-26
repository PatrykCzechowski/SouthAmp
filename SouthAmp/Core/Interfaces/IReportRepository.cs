using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface IReportRepository
    {
        Task<Report> GetByIdAsync(int id);
        Task<IEnumerable<Report>> GetByUserIdAsync(int userId);
        Task AddAsync(Report report);
        Task UpdateAsync(Report report);
        Task DeleteAsync(int id);
        // ...inne metody repozytorium...
    }
}