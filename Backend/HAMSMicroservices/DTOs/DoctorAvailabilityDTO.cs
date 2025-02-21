namespace HAMSMicroservices.DTOs
{
    public class DoctorAvailabilityDTO
    {
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }
        public string DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SlotDurationMinutes { get; set; }
        public int UserId { get; set; }
    }


    public class DoctorSlotRequestDTO
    {
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

}
