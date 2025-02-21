namespace HAMSMicroservices.DTOs
{
    public class AppointmentApprovalDTO
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; } 
        public bool IsApproved { get; set; } 
    }
}
