namespace HAMSMicroservices.DTOs
{
    public class DoctorAvailabilitySlotDTO
    {
        public int DoctorId {  get; set; }
        public TimeSpan SlotStartTime { get; set; }
        public TimeSpan SlotEndTime { get; set; }

    }
    
    public class DoctorSlotsDTO
    {
        public TimeSpan SlotStartTime { get; set; }
        public TimeSpan SlotEndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
