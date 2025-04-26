using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<AuditLog> GetByIdAsync(int id);
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId);
        Task AddAsync(AuditLog log);
        // ...inne metody repozytorium...
    }
}