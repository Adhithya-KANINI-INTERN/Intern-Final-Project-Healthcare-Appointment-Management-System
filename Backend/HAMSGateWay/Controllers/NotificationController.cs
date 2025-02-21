using System.Collections.Generic;
using System.Threading.Tasks;
using HAMSGateway.DTOs;
using HAMSGateWay.DTOs;
using HAMSGateWay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HAMSGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user-notifications/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var notifications = await _notificationService.GetUserNotifications(userId);
            if (notifications == null || notifications.Count == 0)
            {
                return NotFound("No notifications found for this user.");
            }

            return Ok(notifications);
        }

        
        [HttpGet("reminders/{userId}")]
        public async Task<IActionResult> GetUpcomingReminders(int userId)
        {
            var reminders = await _notificationService.GetUpcomingReminders(userId);
            if (reminders == null || reminders.Count == 0)
            {
                return NotFound("No upcoming reminders found.");
            }

            return Ok(reminders);
        }


        [HttpPut("mark-as-read/{notificationId}")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            if (notificationId <= 0)
                return BadRequest("Invalid notification ID.");

            try
            {
                var result = await _notificationService.MarkNotificationAsRead(notificationId);
                if (!result)
                    return StatusCode(500, "Failed to mark the notification as read.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
