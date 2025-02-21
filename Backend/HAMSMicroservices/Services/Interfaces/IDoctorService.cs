using HAMSMicroservices.DTOs;
using HAMSMicroservices.Models;

namespace HAMSMicroservices.Services.Interfaces
{
    public interface IDoctorService
    {

        //Task<bool> CheckDoctorProfile(int userId);
        Task<bool> CheckVerifiedDoctorProfile(int userId);
        Task<DoctorDTO> GetDoctorProfilebyId(int doctorId);
        Task<DoctorDTO> GetDoctorProfile(int userId);
        Task<List<DoctorDTO>> GetAllDoctorProfile();
        Task<List<DoctorDTO>> GetPendingDoctorsAsync();
        Task<bool> AddDoctorProfile(int userId, string specialization, int yearsOfExperience);
        Task<bool> VerifyDoctorProfile(VerifyDoctorDTO verifyDoctorDto);
        Task<bool> UpdateDoctor(DoctorDTO doctorDto);
        Task<bool> ManageAvailability(DoctorAvailabilityDTO availabilityDto);
        Task<List<DoctorDTO>> GetDoctorsBySpecialization(string specialization);
        Task<List<DoctorSlotsDTO>> GetDoctorSlots(int userId, DateTime date);
        Task<List<DoctorAvailabilitySlotDTO>> GetAvailableSlotsofDoctor(int doctorId, DateTime appointmentDate);
    }
}
