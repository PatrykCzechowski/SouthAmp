using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int id);
        // ...inne metody repozytorium...
    }
}