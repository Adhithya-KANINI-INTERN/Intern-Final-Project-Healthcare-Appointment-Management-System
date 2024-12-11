namespace HAMSMicroservices.DTOs
{
    public class AppointmentUpdateDTO
    {
        public int AppointmentId { get; set; }
        public DateTime? NewAppointmentDate { get; set; }
        public string Status { get; set; } 
        public string Reason { get; set; }
    }
}
