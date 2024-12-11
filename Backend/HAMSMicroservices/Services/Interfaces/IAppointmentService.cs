using HAMSMicroservices.DTOs;

namespace HAMSMicroservices.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDTO>> GetAppointmentsByDoctor(int doctorId);
        Task<List<AppointmentDTO>> GetAppointmentsByPatient(int patientId);
        Task<bool> CreateAppointment(AppointmentDTO createDto);
        Task<bool> UpdateAppointment(AppointmentUpdateDTO updateDto);
        Task<bool> CancelAppointment(AppointmentCancelDTO cancelDto);
        Task<bool> ApproveAppointment(int appointmentId, AppointmentApprovalDTO approvalDto);

    }
}
