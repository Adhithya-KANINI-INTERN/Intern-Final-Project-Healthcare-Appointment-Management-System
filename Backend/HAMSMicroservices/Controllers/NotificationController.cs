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

        [Authorize]
        [HttpGet("user-notifications")]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = _notificationService.GetUserIdFromContext();
            if (!userId.HasValue)
                return Unauthorized("User not found.");

            var notifications = await _notificationService.GetNotificationsByUser(userId.Value);
            return Ok(notifications);
        }

        [Authorize]
        [HttpGet("notifications/reminders")]
        public async Task<IActionResult> GetReminders()
        {
            var userId = _notificationService.GetUserIdFromContext(); 

            if (!userId.HasValue)
                return Unauthorized("User not logged in.");

            var reminders = await _notificationService.GetUpcomingRemindersForUser(userId.Value);

            return Ok(reminders);
        }


    }
}
