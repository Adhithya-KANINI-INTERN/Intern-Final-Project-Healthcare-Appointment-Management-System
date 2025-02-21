namespace HAMSMicroservices.DTOs
{
    public class AppointmentUpdateDTO
    {
        public int AppointmentId { get; set; }
        public DateOnly? NewDate { get; set; } 
        public TimeOnly NewStartTime { get; set; }
        public TimeOnly NewEndTime { get; set; }
        public string Status { get; set; } 
        public string Reason { get; set; }
    }
}
