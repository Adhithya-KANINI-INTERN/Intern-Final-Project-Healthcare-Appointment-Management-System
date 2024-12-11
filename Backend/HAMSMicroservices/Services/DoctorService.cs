using HAMSMicroservices.DTOs;
using HAMSMicroservices.Models;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HAMSMicroservices.Services
{
    public class DoctorService : IDoctorService
    {
        
        private readonly AppDBContext _dbContext;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public DoctorService(AppDBContext appDBContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = appDBContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetDoctorIdFromToken()
        {
            var identity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var doctorIdClaim = identity.FindFirst("DoctorId")?.Value;
                return int.TryParse(doctorIdClaim, out var doctorId) ? doctorId : 0;
            }

            throw new Exception("DoctorId not found in the token.");
        }


        public async Task<DoctorDTO> GetDoctorProfile(int doctorId)
        {
            var doctor = await _dbContext.Doctors.Include(d => d.User).SingleOrDefaultAsync(d => d.DoctorId == doctorId);

            if(doctor == null)
            {
                return null;
            }

            return new DoctorDTO
            {
                DoctorId = doctorId,
                DoctorName = doctor.User.FullName,
                Specialization = doctor.Specialization
            };
        }

        public async Task<List<DoctorDTO>> GetDoctorsBySpecialization(string specialization)
        {
            var doctors = await _dbContext.Doctors
                .Include(d => d.User)
                .Where(d => d.Specialization == specialization)
                .Select(d => new DoctorDTO
                {
                    DoctorId = d.DoctorId,
                    DoctorName = d.User.FullName,
                    Specialization = d.Specialization
                })
                .ToListAsync();

            return doctors;
        }

        public async Task<bool> UpdateDoctor(DoctorDTO doctorDto)
        {
            var updateDoctor = await _dbContext.Doctors.Include(u =>  u.User).SingleOrDefaultAsync(u => u.DoctorId == doctorDto.DoctorId);

            if(updateDoctor == null)
            {
                return false;
            }

            updateDoctor.User.FullName = doctorDto.DoctorName ?? updateDoctor.User.FullName;

            updateDoctor.Specialization = doctorDto.Specialization ?? updateDoctor.Specialization;

            _dbContext.Doctors.Update(updateDoctor);

            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<List<DoctorAvailabilitySlotDTO>> GetAvailableSlotsForDoctor(int doctorId, DateTime appointmentDate)
        {
            var dayOfWeek = appointmentDate.DayOfWeek.ToString();
            var timeOnly = TimeOnly.FromDateTime(appointmentDate);

            
            var doctorAvailability = await _dbContext.Doctoravailabilities
                .Where(a => a.DoctorId == doctorId && a.DayOfWeek == dayOfWeek)
                .SingleOrDefaultAsync();

            if (doctorAvailability == null)
            {
                return null; 
            }

            var availableSlots = await _dbContext.Doctoravailabilityslots
                .Where(s => s.AvailabilityId == doctorAvailability.AvailabilityId)
                .ToListAsync();

            return availableSlots.Select(slot => new DoctorAvailabilitySlotDTO
            {
                SlotStartTime = slot.SlotStartTime.ToTimeSpan(),
                SlotEndTime = slot.SlotEndTime.ToTimeSpan()
            }).ToList();
        }



        public async Task<bool> ManageAvailability(DoctorAvailabilityDTO availabilityDto)
        {
            var startTimeOnly = TimeOnly.FromTimeSpan(availabilityDto.StartTime);
            var endTimeOnly = TimeOnly.FromTimeSpan(availabilityDto.EndTime);

            var availability = new Doctoravailability
            {
                DoctorId = availabilityDto.DoctorId,
                DayOfWeek = availabilityDto.DayOfWeek,
                StartTime = startTimeOnly,
                EndTime = endTimeOnly
            };

            _dbContext.Doctoravailabilities.Add(availability);
            await _dbContext.SaveChangesAsync();

            return true;
        }


        //public async Task<bool> UpdateAppointmentStatus(int appointmentId, AppointmentStatusDTO statusDto)
        //{
        //    var appointment = await _dbContext.Appointments.SingleOrDefaultAsync(a => a.AppointmentId == statusDto.AppointmentId);

        //    if (appointment == null)
        //    {
        //        return false;
        //    }

        //    appointment.Status = statusDto.Status;

        //    appointment.Reason = statusDto.Reason;

        //    _dbContext.Appointments.Update(appointment);

        //    await _dbContext.SaveChangesAsync();

        //    return true;
        //}
        
    }
}
