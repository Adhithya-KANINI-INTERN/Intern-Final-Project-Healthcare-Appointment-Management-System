using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HAMSGateWay.DTOs;

namespace HAMSGateWay.Services
{
    public class NotificationService
    {
        private readonly string baseUrl = "https://localhost:7033/api/"; 

        public async Task<List<NotificationDTO>> GetUserNotifications(int userId)
        {
            List<NotificationDTO> notifications = new List<NotificationDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.GetAsync($"Notification/user-notifications/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    notifications = await response.Content.ReadFromJsonAsync<List<NotificationDTO>>();
                }
            }
            return notifications;
        }

        public async Task<List<NotificationDTO>> GetUpcomingReminders(int userId)
        {
            List<NotificationDTO> reminders = new List<NotificationDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.GetAsync($"Notification/reminders/{userId}");
                Console.WriteLine($"{baseUrl}Notification/reminders/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    reminders = await response.Content.ReadFromJsonAsync<List<NotificationDTO>>();
                }
            }
            return reminders;
        }

        public async Task<bool> MarkNotificationAsRead(int notificationId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.PutAsJsonAsync($"Notification/mark-as-read/{notificationId}", notificationId);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
    }
}

