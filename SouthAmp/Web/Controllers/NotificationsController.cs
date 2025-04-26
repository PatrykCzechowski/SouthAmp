using Microsoft.AspNetCore.Mvc;
using SouthAmp.Application.UseCases;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SouthAmp.Web.Models;
using System.Security.Claims;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationUseCases _notificationUseCases;
        private readonly IMapper _mapper;
        public NotificationsController(NotificationUseCases notificationUseCases, IMapper mapper)
        {
            _notificationUseCases = notificationUseCases;
            _mapper = mapper;
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            var userId = int.Parse(userIdStr);
            var notifications = await _notificationUseCases.GetUserNotificationsAsync(userId);
            return Ok(new ApiResponse<IEnumerable<NotificationDto>>(_mapper.Map<IEnumerable<NotificationDto>>(notifications)));
        }

        [HttpPost("{id}/read")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationUseCases.MarkAsReadAsync(id);
            return Ok(new ApiResponse<string>("Notification marked as read"));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
        {
            var notification = _mapper.Map<Notification>(dto);
            var result = await _notificationUseCases.AddNotificationAsync(notification);
            return Ok(new ApiResponse<NotificationDto>(_mapper.Map<NotificationDto>(result)));
        }
    }
}