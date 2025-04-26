using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface INotificationUseCases
    {
        Task<Notification> AddNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}
