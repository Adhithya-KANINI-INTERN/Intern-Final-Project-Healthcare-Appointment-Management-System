using HAMSMicroservices.DTOs;

namespace HAMSMicroservices.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorDTO> GetDoctorProfile(int doctorId);
        Task<bool> UpdateDoctor(DoctorDTO doctorDto);
        //Task<bool> UpdateAppointmentStatus(int appointmentId, AppointmentStatusDTO statusDto);
        Task<bool> ManageAvailability(DoctorAvailabilityDTO availabilityDto);
        Task<List<DoctorDTO>> GetDoctorsBySpecialization(string specialization);
        Task<List<DoctorAvailabilitySlotDTO>> GetAvailableSlotsForDoctor(int doctorId, DateTime appointmentDate);
        int GetDoctorIdFromToken();
    }
}
