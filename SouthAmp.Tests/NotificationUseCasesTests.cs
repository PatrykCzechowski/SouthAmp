using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.Tests
{
    public class NotificationUseCasesTests
    {
        private readonly Mock<INotificationRepository> _notificationRepoMock = new();
        private readonly NotificationUseCases _sut;

        public NotificationUseCasesTests()
        {
            _sut = new NotificationUseCases(_notificationRepoMock.Object);
        }

        [Fact]
        public async Task AddNotificationAsync_SetsDefaultsAndAdds()
        {
            var notification = new Notification { Id = 1 };
            _notificationRepoMock.Setup(r => r.AddAsync(notification)).Returns(Task.CompletedTask);
            var result = await _sut.AddNotificationAsync(notification);
            Assert.False(result.IsRead);
            Assert.True((DateTime.UtcNow - result.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public async Task GetUserNotificationsAsync_ReturnsNotifications()
        {
            var notifications = new List<Notification> { new Notification { Id = 1 } };
            _notificationRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(notifications);
            var result = await _sut.GetUserNotificationsAsync(1);
            Assert.Single(result);
        }

        [Fact]
        public async Task MarkAsReadAsync_UpdatesNotification_WhenFound()
        {
            var notification = new Notification { Id = 1, IsRead = false };
            _notificationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(notification);
            _notificationRepoMock.Setup(r => r.UpdateAsync(notification)).Returns(Task.CompletedTask);
            await _sut.MarkAsReadAsync(1);
            Assert.True(notification.IsRead);
        }

        [Fact]
        public async Task MarkAsReadAsync_DoesNothing_WhenNotFound()
        {
            _notificationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Notification)null);
            await _sut.MarkAsReadAsync(1);
            _notificationRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Notification>()), Times.Never);
        }
    }
}
