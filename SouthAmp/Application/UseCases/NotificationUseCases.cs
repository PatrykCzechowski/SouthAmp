using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Application.Interfaces;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class NotificationUseCases : INotificationUseCases
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationUseCases(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            notification.CreatedAt = System.DateTime.UtcNow;
            notification.IsRead = false;
            await _notificationRepository.AddAsync(notification);
            return notification;
        }
        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }
        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _notificationRepository.UpdateAsync(notification);
            }
        }
    }
}