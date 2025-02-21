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

        private readonly INotificationService _notificationService;

        public AppointmentService(AppDBContext appDBContext, INotificationService notificationService)
        {
            _dbContext = appDBContext;
            _notificationService = notificationService;
        }


        private async Task<int?> GetDoctorIdAsync(int userId)
        {
            
            var doctor = await _dbContext.Doctors
                .SingleOrDefaultAsync(d => d.UserId == userId);

            if (doctor == null)
                return null; 

            return doctor.DoctorId;
        }

        public async Task<int> GetTotalConfirmedAppointments()
        {
            var appointments =  await _dbContext.Appointments.CountAsync(a => a.Status == "Confirmed");
            return appointments;
        }

        public async Task<List<AppointmentDTO>> GetAllAppointments()
        {
            var appointments = await _dbContext.Appointments
                .Include(a => a.Patient) 
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User) 
                .Where(a => a.Status == "Confirmed") 
                .Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName, 
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.User.FullName, 
                    Date = a.Date,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    Reason = a.ReasonForVisit
                })
                .ToListAsync();

            return appointments;
        }


        public async Task<List<AppointmentDTO>> GetDoctorAppointment(int doctorId)
        {
            var appointments = await _dbContext.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    //PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.User.FullName,
                    Date = a.Date,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    Reason = a.ReasonForVisit
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByDoctor(int userId)
        {
            var doctorId = await GetDoctorIdAsync(userId);
            

            var appointments = await _dbContext.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(a => a.User)
                .Where(a => a.DoctorId == doctorId)
                .Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.User.FullName,
                    Date = a.Date,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    Reason = a.ReasonForVisit
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
                    Date = a.Date,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    Reason = a.ReasonForVisit
                })
                .ToListAsync();

            return appointments;

        }

        public async Task<int> GetTotalAppointments(int userId, bool isAdmin = false)
        {
            if (isAdmin)
            {
                
                var totalAppointments = await _dbContext.Appointments
                    .CountAsync(a => a.Status == "Confirmed");

                return totalAppointments;
            }
            else
            {
                
                var doctorId = await GetDoctorIdAsync(userId);

                var totalAppointments = await _dbContext.Appointments
                    .CountAsync(a => a.DoctorId == doctorId && (a.Status == "Confirmed"));

                return totalAppointments;
            }
        }


        public async Task<bool> IsDoctorAvailable(int doctorId, DateOnly appointmentDate, TimeOnly startTime, TimeOnly endTime)
        {
            var availability = await _dbContext.Doctoravailability
                .Include(a => a.Doctoravailabilityslots)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId && d.Date == appointmentDate);

            if (availability == null) return false;

            return availability.Doctoravailabilityslots
                .Any(slot =>
                    slot.IsAvailable &&
                    slot.SlotStartTime <= startTime &&
                    slot.SlotEndTime >= endTime);
        }






        public async Task<bool> CreateAppointment(AppointmentCreateDTO createDto)
        {

            if (!await IsDoctorAvailable(createDto.DoctorId, createDto.Date, createDto.StartTime, createDto.EndTime))
                return false;


            var doctor = await _dbContext.Doctors
                .Where(d => d.DoctorId == createDto.DoctorId)
                .Join(_dbContext.Users,
                doctor => doctor.UserId,
                user => user.UserId,
                (doctor, user) => new { DoctorID = doctor.DoctorId, FullName = user.FullName, DoctorUserId = doctor.UserId })
                .FirstOrDefaultAsync();

            var patient = await _dbContext.Users
                .Where(u => u.UserId == createDto.PatientId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync();


            var appointment = new Appointment
            {
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                Date = createDto.Date,
                StartTime = createDto.StartTime,
                EndTime = createDto.EndTime,
                Status = "Confirmed ",
                ReasonForVisit = createDto.Reason
            };

            _dbContext.Appointments.Add(appointment);
            await _dbContext.SaveChangesAsync();

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = createDto.PatientId,
                Message = $"Your appointment with Doctor {doctor.FullName} is Confirmed.",
                NotificationType = "AppointmentCreated",
                AppointmentId = appointment.AppointmentId
            });



            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = doctor.DoctorUserId,
                Message = $" Patient [{patient}] has booked an appointment with you. Appointment details: {appointment.Date}.",
                NotificationType = "AppointmentCreated",
                AppointmentId = appointment.AppointmentId
            });

            //var slot = await _dbContext.Doctoravailabilityslots
            //    .FirstOrDefaultAsync(s =>
            //        s.AvailabilityId == createDto.DoctorId &&
            //        s.SlotStartTime <= createDto.StartTime &&
            //        s.SlotEndTime >= createDto.EndTime);

            //if (slot != null)
            //{
            //    slot.IsAvailable = false;
            //    _dbContext.Doctoravailabilityslots.Update(slot);
            //    await _dbContext.SaveChangesAsync();
            //}

            var slot = await _dbContext.Doctoravailabilityslots
                .Join(_dbContext.Doctoravailability,
                    slot => slot.AvailabilityId,
                    availability => availability.AvailabilityId,
                    (slot, availability) => new { Slot = slot, Availability = availability })
                .Where(joined =>
                    joined.Availability.DoctorId == createDto.DoctorId &&
                    joined.Availability.Date == createDto.Date &&
                    joined.Slot.SlotStartTime <= createDto.StartTime &&
                    joined.Slot.SlotEndTime >= createDto.EndTime &&
                    joined.Slot.IsAvailable)
                .Select(joined => joined.Slot)
                .FirstOrDefaultAsync();

            if (slot != null)
            {
                slot.IsAvailable = false;
                _dbContext.Doctoravailabilityslots.Update(slot);
                await _dbContext.SaveChangesAsync();
            }


            return true;
        }



        public async Task<bool> UpdateAppointment(AppointmentUpdateDTO updateDto)
        {

            var appointment = await _dbContext.Appointments.SingleOrDefaultAsync(a => a.AppointmentId == updateDto.AppointmentId);
            if (appointment == null)
            {
                return false; 
            }


            if (updateDto.NewDate.HasValue)
            {
                bool isAvailable = await IsDoctorAvailable(appointment.DoctorId, updateDto.NewDate.Value, updateDto.NewStartTime, updateDto.NewEndTime);
                if (!isAvailable) 
                {
                    return false; 
                }

                appointment.Date = updateDto.NewDate.Value;

                await _notificationService.CreateNotification(new CreateNotificationDTO
                {
                    UserId = appointment.PatientId,
                    Message = $"Your appointment has been rescheduled to {updateDto.NewDate.Value}.",
                    NotificationType = "AppointmentRescheduled",
                    AppointmentId = appointment.AppointmentId
                });

                await _notificationService.CreateNotification(new CreateNotificationDTO
                {
                    UserId = appointment.DoctorId,
                    Message = $"An appointment has been rescheduled to {updateDto.NewDate.Value}.",
                    NotificationType = "AppointmentRescheduled",
                    AppointmentId = appointment.AppointmentId
                });
            }

            
            if (!string.IsNullOrEmpty(updateDto.Status))
            {
                appointment.Status = updateDto.Status;
            }

            if (!string.IsNullOrEmpty(updateDto.Reason))
            {
                appointment.ReasonForVisit = updateDto.Reason;
            }


            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();

            return true; 
        }


        public async Task<bool> CancelAppointment(AppointmentCancelDTO cancelDto)
        {
            var appointment = await _dbContext.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == cancelDto.AppointmentId);

            if (appointment == null || appointment.Status == "Cancelled")
            {
                return false; 
            }

            appointment.Status = "Cancelled";
            appointment.ReasonForVisit = cancelDto.Reason;

            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = appointment.PatientId,
                Message = $"Your appointment on {appointment.Date} with Dr. {appointment.Doctor.User.FullName} has been cancelled.",
                NotificationType = "AppointmentCancelled",
                AppointmentId = appointment.AppointmentId
            });

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = appointment.DoctorId,
                Message = $"The appointment on {appointment.Date} with a patient has been cancelled.",
                NotificationType = "AppointmentCancelled",
                AppointmentId = appointment.AppointmentId
            });

            var doctorAvailability = await _dbContext.Doctoravailability
                .FirstOrDefaultAsync(d => d.DoctorId == appointment.DoctorId && d.Date == appointment.Date);

            if (doctorAvailability != null)
            {
              
                var slot = await _dbContext.Doctoravailabilityslots
                    .FirstOrDefaultAsync(s =>
                        s.AvailabilityId == doctorAvailability.AvailabilityId &&
                        s.SlotStartTime <= appointment.StartTime &&
                        s.SlotEndTime >= appointment.EndTime);

                if (slot != null && !slot.IsAvailable)
                {
                    
                    slot.IsAvailable = true;
                    _dbContext.Doctoravailabilityslots.Update(slot);
                    await _dbContext.SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<bool> CancelAppointmentByDoctor(AppointmentCancelDTO cancelDto)
        {
            var appointment = await _dbContext.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == cancelDto.AppointmentId);

            if (appointment == null || appointment.Status == "Cancelled")
                return false; 

            appointment.Status = "Cancelled";
            appointment.Cancellation = cancelDto.Reason;

            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = appointment.PatientId,
                Message = $"Your appointment on {appointment.Date} with Dr. {appointment.Doctor.User.FullName} has been cancelled by the doctor.",
                NotificationType = "AppointmentCancelled",
                AppointmentId = appointment.AppointmentId
            });

            return true;
        }


        public async Task<bool> MarkAppointmentAsCompleted(int appointmentId)
        {

            var appointment = await _dbContext.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                Console.WriteLine($"Appointment with ID {appointmentId} not found.");
                return false;
            }

            if (appointment.Status != "Confirmed")
            {
                Console.WriteLine($"Appointment status is '{appointment.Status}', not 'Confirmed'.");
                return false;
            }

            var doctor = await _dbContext.Doctors
                .Include(d => d.User) 
                .FirstOrDefaultAsync(d => d.DoctorId == appointment.DoctorId);

            if (doctor == null)
            {
                Console.WriteLine($"Doctor with ID {appointment.DoctorId} not found.");
                return false;
            }

            string doctorName = doctor.User.FullName; 

            appointment.Status = "Completed";

            _dbContext.Appointments.Update(appointment);
            await _dbContext.SaveChangesAsync();

            await _notificationService.CreateNotification(new CreateNotificationDTO
            {
                UserId = appointment.PatientId,
                Message = $"Your appointment on {appointment.Date} with Dr. {doctorName} has been completed.",
                NotificationType = "AppointmentCompleted",
                AppointmentId = appointment.AppointmentId
            });

            return true;
        }
    }
}
