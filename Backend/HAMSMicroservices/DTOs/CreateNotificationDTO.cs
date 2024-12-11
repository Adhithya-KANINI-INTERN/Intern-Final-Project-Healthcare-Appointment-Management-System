namespace HAMSMicroservices.DTOs
{
    public class CreateNotificationDTO
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public string NotificationType { get; set; }
        public int? AppointmentId { get; set; }
    }
}
