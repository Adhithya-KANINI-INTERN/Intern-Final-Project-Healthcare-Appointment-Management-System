using HAMSMicroservices.DTOs;
using HAMSMicroservices.Models;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HAMSMicroservices.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDBContext _dbContext;

        private readonly HttpContextAccessor _httpContextAccessor;

        public NotificationService(AppDBContext dbContext, HttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<NotificationDTO>> GetNotificationsByUser(int userId)
        {
            return await _dbContext.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    UserId = n.UserId,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    NotificationType = n.NotificationType,
                    AppointmentId = n.AppointmentId
                })
                .ToListAsync();
        }

        public async Task CreateNotification(CreateNotificationDTO notificationDto)
        {
            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                Message = notificationDto.Message,
                NotificationType = notificationDto.NotificationType,
                AppointmentId = notificationDto.AppointmentId
            };

            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _dbContext.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _dbContext.Notifications.Update(notification);
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<List<NotificationDTO>> GetUpcomingRemindersForUser(int userId)
        {

            var currentTime = DateTime.UtcNow;
            var twoHoursLater = currentTime.AddHours(2);

            // Fetch upcoming appointments for the user within the next 2 hours
            var upcomingAppointments = await _dbContext.Appointments
                .Where(a => a.PatientId == userId
                            && a.AppointmentDate >= currentTime
                            && a.AppointmentDate <= twoHoursLater
                            && a.Status == "Confirmed")
                .ToListAsync();

            var reminders = new List<NotificationDTO>();

            foreach (var appointment in upcomingAppointments)
            {
                var existingReminder = await _dbContext.Notifications
                    .AnyAsync(n => n.UserId == userId
                                   && n.AppointmentId == appointment.AppointmentId
                                   && n.NotificationType == "Reminder");

                if (!existingReminder)
                {
                    var message = $"Reminder: Your appointment with Doctor {appointment.DoctorId} is scheduled for {appointment.AppointmentDate}.";
                    var notification = new Notification
                    {
                        UserId = userId,
                        AppointmentId = appointment.AppointmentId,
                        Message = message,
                        NotificationType = "Reminder",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false
                    };

                    _dbContext.Notifications.Add(notification);
                    reminders.Add(new NotificationDTO
                    {
                        Message = message,
                        CreatedAt = notification.CreatedAt,
                        NotificationType = notification.NotificationType
                    });
                }
            }

            await _dbContext.SaveChangesAsync();

            return reminders;
        }


        public int? GetUserIdFromContext()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User;

            if (userClaims == null)
                return null;

            var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out var userId))
                return userId;

            return null; 
        }

    }
}
