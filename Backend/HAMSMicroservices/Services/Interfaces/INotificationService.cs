using HAMSMicroservices.DTOs;

namespace HAMSMicroservices.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetNotificationsByUser(int userId);
        Task CreateNotification(CreateNotificationDTO notificationDto);
        Task MarkAsRead(int notificationId);
        Task<List<NotificationDTO>> GetUpcomingRemindersForUser(int userId);
    }
}
