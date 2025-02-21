namespace HAMSGateWay.DTOs
{
    public class CreateNotificationDTO
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public string NotificationType { get; set; }
        public int AppointmentId { get; set; }
    }

    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string NotificationType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

