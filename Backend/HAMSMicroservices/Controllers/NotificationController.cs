using HAMSMicroservices.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HAMSMicroservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user-notifications/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid user ID.");

            var notifications = await _notificationService.GetNotificationsByUser(userId);

            if (notifications == null || notifications.Count == 0)
                return NotFound("No notifications found for the user.");

            return Ok(notifications);
        }

        [HttpGet("reminders/{userId}")]
        public async Task<IActionResult> GetReminders(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid user ID.");

            var reminders = await _notificationService.GetUpcomingRemindersForUser(userId);

            if (reminders == null || reminders.Count == 0)
                return NotFound("No reminders found.");

            return Ok(reminders);
        }

        [HttpPut("mark-as-read/{notificationId}")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            if (notificationId <= 0)
                return BadRequest("Invalid notification ID.");

            try
            {
                await _notificationService.MarkAsRead(notificationId);
                return Ok("Notification marked as read.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}
