using HAMSMicroservices.DTOs;
using HAMSMicroservices.Models;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Claims;

namespace HAMSMicroservices.Services
{
    public class AppointmentService : IAppointmentService
    {

        private readonly AppDBContext _dbContext;

        private readonly HttpContextAccessor _httpContextAccessor;

        private readonly INotificationService _notificationService;

        public AppointmentService(AppDBContext appDBContext, HttpContextAccessor  httpContextAccessor, NotificationService notificationService)
        {
            _dbContext = appDBContext;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
        }




        public async Task<List<AppointmentDTO>> GetAppointmentsByDoctor(int doctorId)
        {
            var appointments = await _dbContext.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(a => a.User)
                .Where(a => a.DoctorId == doctorId)
                .Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.User.FullName,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    Reason = a.Reason
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByPatient(int patientId)
        {
            var appointments = await _dbContext.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .Where(a => a.PatientId == patientId)
                .Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.User.FullName,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    Reason = a.Reason
                })
                .ToListAsync();

            return appointments;

        }


        public async Task<bool> IsDoctorAvailable(int doctorId, DateTime appointmentDate)
        {
            // Extract the day of the week and the time from the appointment date
            var dayOfWeek = appointmentDate.DayOfWeek.ToString();
            var timeOnly = TimeOnly.FromDateTime(appointmentDate);  // Get the time only from the appointment datetime

            // Check if the doctor has availability for the selected day
            var doctorAvailability = await _dbContext.Doctoravailabilities
                .Where(d => d.DoctorId == doctorId && d.DayOfWeek == dayOfWeek)
                .SingleOrDefaultAsync();

            if (doctorAvailability == null)
            {
                return false; 
            }

            // Check if the appointment time falls within the doctor's available slots
            var availableSlots = await _dbContext.Doctoravailabilityslots
                .Where(s => s.AvailabilityId == doctorAvailability.AvailabilityId)
                .ToListAsync();

            var isSlotAvailable = availableSlots.Any(slot => timeOnly >= slot.SlotStartTime && timeOnly <= slot.SlotEndTime);

            if (!isSlotAvailable)
            {
                return false; 
            }

            // Check if the time slot is already booked by another patient
            var existingAppointment = await _dbContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate && a.Status != "Cancelled")
                .FirstOrDefaultAsync();

            if (existingAppointment != null)
            {
                return false; 
            }

            return true; 
        }

        private int? GetPatientIdFromContext()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User;

            if (userClaims == null)
                return null;

            // Ensure the role is "Patient"
            var roleClaim = userClaims.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaim != "Patient")
                return null;

            // Get the PatientId (assumes it's stored as a claim)
            var patientIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(patientIdClaim, out var patientId))
                return patientId;

            return null;
        }

        public async Task<bool> CreateAppointment(AppointmentDTO createDto)
        {
            if (createDto.PatientId == 0)
            {
                var patientId = GetPatientIdFromContext();
                if (patientId == null)
                    throw new UnauthorizedAccessException("You must be logged in as a patient to create an appointment.");

                createDto.PatientId = patientId.Value;
            }

            bool isAvailable = await IsDoctorAvailable(createDto.DoctorId, createDto.AppointmentDate);

            if (!isAvailable)
            {
                return false; 
            }

            var doctor = await _dbContext.Users
                .Where(u => u.UserId == createDto.DoctorId && u.Role == "Doctor")
                .FirstOrDefaultAsync();

            // Create the appointment
            var appointment = new Appointment
            {
                PatientId = createDto.PatientId,  // Patient ID is retrieved from the JWT token or passed as part of the DTO
                DoctorId = createDto.DoctorId,
                AppointmentDate = createDto.AppointmentDate,
                Status = "Pending", // Initially, set status as pending
                Reason = createDto.Reason
            };

            // Add the appointment to the database
            _dbContext.Appointments.Add(appointment);
            await _dbContext.SaveChangesAsync();

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = createDto.PatientId,
                Message = $"Your appointment with Doctor {doctor.FullName} is pending approval.",
                NotificationType = "AppointmentCreated",
                AppointmentId = appointment.AppointmentId
            });

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = createDto.DoctorId,
                Message = $"A patient has booked an appointment with you. Appointment details: {appointment.AppointmentDate}.",
                NotificationType = "AppointmentCreated",
                AppointmentId = appointment.AppointmentId
            });


            return true;
        }

        public async Task<bool> UpdateAppointment(AppointmentUpdateDTO updateDto)
        {
            var appointment = await _dbContext.Appointments.SingleOrDefaultAsync( a => a.AppointmentId == updateDto.AppointmentId);
            if (appointment == null)
            {
                return false;
            }

            if(updateDto.NewAppointmentDate.HasValue)
            {
                var doctorAvailability = await _dbContext.Doctoravailabilities
                    .Where(a => a.DoctorId == appointment.DoctorId
                                && a.DayOfWeek == updateDto.NewAppointmentDate.Value.DayOfWeek.ToString()
                                && a.StartTime <= TimeOnly.FromTimeSpan(updateDto.NewAppointmentDate.Value.TimeOfDay)
                                && a.EndTime >= TimeOnly.FromTimeSpan(updateDto.NewAppointmentDate.Value.TimeOfDay))
                    .FirstOrDefaultAsync();

                if (doctorAvailability == null)
                {
                    throw new ValidationException("Doctor is unavailable at the requested time.");
                }

                appointment.AppointmentDate = updateDto.NewAppointmentDate.Value;
            }

            //if (!string.IsNullOrEmpty(updateDto.Status))
            //{
            //    appointment.Status = updateDto.Status; 
            //}

            if (!string.IsNullOrEmpty(updateDto.Reason))
            {
                appointment.Reason = updateDto.Reason; 
            }

            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();

            return true;

        }


        //public async Task<bool> UpdateAppointment(AppointmentUpdateDTO updateDto)
        //{
        //    // Check if the appointment exists
        //    var appointment = await _dbContext.Appointments.SingleOrDefaultAsync(a => a.AppointmentId == updateDto.AppointmentId);
        //    if (appointment == null)
        //    {
        //        return false; // Appointment does not exist
        //    }

        //    // Check if a new appointment date is being set and validate availability
        //    if (updateDto.NewAppointmentDate.HasValue)
        //    {
        //        bool isAvailable = await IsDoctorAvailable(appointment.DoctorId, updateDto.NewAppointmentDate.Value);
        //        if (!isAvailable)
        //        {
        //            return false; // Doctor is not available at the new time
        //        }

        //        appointment.AppointmentDate = updateDto.NewAppointmentDate.Value; // Update the date

        //        await _notificationService.CreateNotification(new CreateNotificationDTO
        //        {
        //            UserId = appointment.PatientId,
        //            Message = $"Your appointment has been rescheduled to {updateDto.NewAppointmentDate.Value}.",
        //            NotificationType = "AppointmentRescheduled",
        //            AppointmentId = appointment.AppointmentId
        //        });

        //        await _notificationService.CreateNotification(new CreateNotificationDTO
        //        {
        //            UserId = appointment.DoctorId,
        //            Message = $"An appointment has been rescheduled to {updateDto.NewAppointmentDate.Value}.",
        //            NotificationType = "AppointmentRescheduled",
        //            AppointmentId = appointment.AppointmentId
        //        });
        //    }

        //    // Update the appointment status if provided
        //    if (!string.IsNullOrEmpty(updateDto.Status))
        //    {
        //        appointment.Status = updateDto.Status;
        //    }

        //    // Update the reason if provided
        //    if (!string.IsNullOrEmpty(updateDto.Reason))
        //    {
        //        appointment.Reason = updateDto.Reason;
        //    }

        //    // Save changes to the database
        //    _dbContext.Appointments.Update(appointment);
        //    await _dbContext.SaveChangesAsync();

        //    return true; // Successfully updated the appointment
        //}


        public async Task<bool> CancelAppointment(AppointmentCancelDTO cancelDto)
        {
            var appointment = await _dbContext.Appointments.SingleOrDefaultAsync(a => a.AppointmentId == cancelDto.AppointmentId);
            if (appointment == null)
                return false; 

            appointment.Status = "Cancelled";
            appointment.Reason = cancelDto.Reason;

            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = appointment.PatientId,
                Message = "Your appointment has been cancelled.",
                NotificationType = "AppointmentCancelled",
                AppointmentId = appointment.AppointmentId
            });

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = appointment.DoctorId,
                Message = "An appointment has been cancelled.",
                NotificationType = "AppointmentCancelled",
                AppointmentId = appointment.AppointmentId
            });

            return true;
        }

        public async Task<bool> ApproveAppointment(int appointmentId, AppointmentApprovalDTO approvalDto)
        {
            var appointment = await _dbContext.Appointments.SingleOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null || appointment.DoctorId != approvalDto.DoctorId)
            {
                return false; // Appointment not found or unauthorized doctor
            }

            if (appointment.Status != "Pending")
            {
                return false; // Can only approve pending appointments
            }

            // Update the status to approved or rejected based on the approval
            appointment.Status = approvalDto.IsApproved ? "Confirmed" : "Rejected";

            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();

            // Notify the patient
            string message = approvalDto.IsApproved
                ? "Your appointment has been confirmed by the doctor."
                : "Your appointment has been rejected by the doctor.";

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = appointment.PatientId,
                Message = message,
                NotificationType = approvalDto.IsApproved ? "AppointmentConfirmed" : "AppointmentRejected",
                AppointmentId = appointment.AppointmentId
            });

            return true;
        }

    }
}
