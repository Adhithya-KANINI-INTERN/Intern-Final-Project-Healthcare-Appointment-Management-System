namespace HAMSMicroservices.DTOs
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }

        public string PatientName { get; set; } 
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateOnly Date { get; set; } 
        public TimeOnly StartTime { get; set; } 
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }

    }

    public class AppointmentCreateDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Reason { get; set; }
    }
}
