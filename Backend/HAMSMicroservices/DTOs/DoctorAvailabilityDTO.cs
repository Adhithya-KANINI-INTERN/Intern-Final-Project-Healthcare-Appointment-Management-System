namespace HAMSMicroservices.DTOs
{
    public class DoctorAvailabilityDTO
    {
        public int DoctorId { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
