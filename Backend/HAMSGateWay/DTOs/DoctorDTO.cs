namespace HAMSGateWay.DTOs
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string Token { get; set; }
        public int YearsOfExperience { get; set; }

        public string VerificationStatus { get; set; }
    }

    public class DoctorAvailabilityDTO
    {
        public int DoctorId { get; set; }
        public string DayOfWeek { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDurationMinutes { get; set; }
        public int UserId { get; set; }
    }

    public class DoctorAvailabilitySlotDTO
    {
        public int DoctorId { get; set; }
        public TimeSpan SlotStartTime { get; set; }
        public TimeSpan SlotEndTime { get; set; }

    }

    public class SpecializationDTO
    {
        public string Specialization { get; set; }
        public string Token { get; set; }
    }

    public class AddDoctorProfileDTO
    {
        public int UserId { get; set; }
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
    }

    public class VerifyDoctorDTO
    {
        public int DoctorId { get; set; }
        public string Status { get; set; }
    }

    public class DoctorSlotRequestDTO
    {
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

    public class DoctorSlotsDTO
    {
        public TimeSpan SlotStartTime { get; set; }
        public TimeSpan SlotEndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}

