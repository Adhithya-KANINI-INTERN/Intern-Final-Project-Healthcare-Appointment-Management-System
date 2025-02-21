using HAMSMicroservices.DTOs;

namespace HAMSMicroservices.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<int> GetTotalConfirmedAppointments();

        Task<List<AppointmentDTO>> GetAllAppointments();
        Task<List<AppointmentDTO>> GetDoctorAppointment(int doctorId);
        Task<List<AppointmentDTO>> GetAppointmentsByDoctor(int userId);
        Task<List<AppointmentDTO>> GetAppointmentsByPatient(int patientId);
        Task<bool> CreateAppointment(AppointmentCreateDTO createDto);
        Task<bool> UpdateAppointment(AppointmentUpdateDTO updateDto);
        Task<bool> CancelAppointment(AppointmentCancelDTO cancelDto);
        Task<bool> CancelAppointmentByDoctor(AppointmentCancelDTO cancelDto);
        Task<bool> MarkAppointmentAsCompleted(int appointmentId);

    }
}
