using HAMSMicroservices.DTOs;
using HAMSMicroservices.Models;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;

namespace HAMSMicroservices.Services
{
    public class DoctorService : IDoctorService
    {
        
        private readonly AppDBContext _dbContext;

        private readonly ILogger<DoctorService> _logger;

        public DoctorService(AppDBContext appDBContext, ILogger<DoctorService> logger)
        {
            _dbContext = appDBContext;
            _logger = logger;
        }

        private async Task<int?> GetDoctorIdAsync(int userId)
        {
          
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                _logger.LogError("No user found with ID {UserId}", userId);
                throw new UnauthorizedAccessException("User does not exist.");
            }

            if (user.Role != "Doctor")
            {
                _logger.LogError("User with ID {UserId} does not have the required Doctor role.", userId);
                throw new UnauthorizedAccessException("User does not have the required Doctor role.");
            }

            // Retrieve the DoctorID associated with the UserID
            var doctor = await _dbContext.Doctors.SingleOrDefaultAsync(d => d.UserId == userId);
            if (doctor == null)
            {
                _logger.LogError("No doctor profile found for UserId {UserId}", userId);
                throw new UnauthorizedAccessException("Doctor profile not found for the authenticated user.");
            }

            _logger.LogInformation("Successfully retrieved DoctorId: {DoctorId}", doctor.DoctorId);
            return doctor.DoctorId;
        }



        //public async Task<bool> CheckDoctorProfile(int userId)
        //{
        //    var doctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);

        //    if(doctor == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public async Task<bool> CheckVerifiedDoctorProfile(int userId)
        {
            var isVerified = await _dbContext.Doctors
                .Where(i => i.UserId == userId && i.VerificationStatus == "Verified")
                .FirstOrDefaultAsync();

            if(isVerified == null)
            {
                return false;
            }

            return true;
        }

        public async Task<DoctorDTO> GetDoctorProfilebyId(int doctorId)
        {
           
            var doctor = await _dbContext.Doctors.Include(d => d.User).SingleOrDefaultAsync(d => d.DoctorId == doctorId);

            if (doctor == null)
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

        public async Task<DoctorDTO> GetDoctorProfile(int userId)
        {

            var doctorId = await GetDoctorIdAsync(userId);

            var doctor = await _dbContext.Doctors.Include(d => d.User).SingleOrDefaultAsync(d => d.DoctorId == doctorId);

            if (doctor == null)
            {
                return null;
            }

            return new DoctorDTO
            {
                DoctorId = doctorId.Value,
                DoctorName = doctor.User.FullName,
                Specialization = doctor.Specialization
            };
        }

        public async Task<List<DoctorDTO>> GetAllDoctorProfile()
        {

            try
            {
                var doctors = await _dbContext.Doctors
            .Where(d => d.VerificationStatus == "Verified")
            .Join(_dbContext.Users,
                  doctor => doctor.UserId,
                  user => user.UserId,
                  (doctor, user) => new DoctorDTO
                  {
                      DoctorId = doctor.DoctorId,
                      DoctorName = user.FullName,
                      //Email = user.Email,
                      UserId = user.UserId,
                      Specialization = doctor.Specialization,
                      YearsOfExperience = doctor.YearsOfExperience,
                      VerificationStatus = doctor.VerificationStatus,
                      //CreatedAt = doctor.CreatedAt,
                      //UpdatedAt = doctor.UpdatedAt
                  })
                  .ToListAsync();
                return doctors;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public async Task<List<DoctorDTO>> GetPendingDoctorsAsync()
        {
            var doctors = await _dbContext.Doctors
                .Include(d => d.User) 
                .Where(d => d.VerificationStatus == "Pending")
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            
            if (doctors == null || !doctors.Any())
            {
                return new List<DoctorDTO>();
            }

            return doctors.Select(doctor => new DoctorDTO
            {
                DoctorId = doctor.DoctorId,
                UserId = doctor.UserId,
                DoctorName = doctor.User?.FullName, 
                Specialization = doctor.Specialization,
                VerificationStatus = doctor.VerificationStatus,
                YearsOfExperience = doctor.YearsOfExperience
            }).ToList();
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
                    Specialization = d.Specialization,
                    YearsOfExperience = d.YearsOfExperience
                })
                .ToListAsync();

            return doctors;
        }

        public async Task<bool> AddDoctorProfile(int userId, string specialization, int yearsOfExperience)
        {
           
            _logger.LogInformation("Attempting to add a doctor profile for user {UserId} with specialization {Specialization}.", userId, specialization);


            var existingDoctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (existingDoctor != null)
            {
                throw new InvalidOperationException("Doctor profile already exists.");
            }


            var newDoctor = new Doctor
            {
                UserId = userId,
                Specialization = specialization,
                YearsOfExperience = yearsOfExperience,
                VerificationStatus = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.Doctors.AddAsync(newDoctor);
            await _dbContext.SaveChangesAsync();

            return true;

        }



        public async Task<bool> UpdateDoctor(DoctorDTO doctorDto)
        {
            var doctorId = await GetDoctorIdAsync(doctorDto.UserId);

            if (doctorId == null)
            {
                throw new UnauthorizedAccessException("DoctorId could not be determined from token.");
            }

            doctorDto.DoctorId = doctorId.Value;

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

        public async Task<List<DoctorSlotsDTO>> GetDoctorSlots(int doctorId, DateTime date)
        {
            var doctorSlots = await _dbContext.Doctoravailability
                .Where(u => u.DoctorId == doctorId && u.Date == DateOnly.FromDateTime(date))
                .SingleOrDefaultAsync();

            if (doctorSlots == null)
            {
                return new List<DoctorSlotsDTO>();
            }

            var availableSlots = await _dbContext.Doctoravailabilityslots
                .Where(s => s.AvailabilityId == doctorSlots.AvailabilityId)
                .ToListAsync();

            if (!availableSlots.Any())
            {
                return new List<DoctorSlotsDTO>();
            }

            var slotDTOs = availableSlots.Select(slot => new DoctorSlotsDTO
            {
                SlotStartTime = slot.SlotStartTime.ToTimeSpan(),
                SlotEndTime = slot.SlotEndTime.ToTimeSpan(),
                IsAvailable = slot.IsAvailable
            }).ToList();

            return slotDTOs;
        }

        public async Task<List<DoctorAvailabilitySlotDTO>> GetAvailableSlotsofDoctor(int doctorId, DateTime appointmentDate)
        {

            var doctorAvailability = await _dbContext.Doctoravailability
                .Where(u => u.DoctorId == doctorId && u.Date == DateOnly.FromDateTime(appointmentDate))
                .SingleOrDefaultAsync();

            if (doctorAvailability == null)
            {
                return new List<DoctorAvailabilitySlotDTO>(); 
            }

            var availableSlots = await _dbContext.Doctoravailabilityslots
                .Where(s => s.AvailabilityId == doctorAvailability.AvailabilityId && s.IsAvailable)
                .ToListAsync();

            if (!availableSlots.Any())
            {
                return new List<DoctorAvailabilitySlotDTO>();
            }

            var slotDTOs = availableSlots.Select(slot => new DoctorAvailabilitySlotDTO
            {
                SlotStartTime = slot.SlotStartTime.ToTimeSpan(),
                SlotEndTime = slot.SlotEndTime.ToTimeSpan()
            }).ToList();

            return slotDTOs;
        }


        public async Task<bool> ManageAvailability(DoctorAvailabilityDTO availabilityDto)
        {

            var doctorId = await GetDoctorIdAsync(availabilityDto.UserId);

            if (doctorId == null)
            {
                throw new UnauthorizedAccessException("DoctorId could not be determined from token.");
            }

            if (availabilityDto.SlotDurationMinutes <= 0)
            {
                throw new ArgumentException("Slot duration must be greater than zero.");
            }
                

            availabilityDto.DoctorId = doctorId.Value;


            var startTimeOnly = availabilityDto.StartTime;
            var endTimeOnly = availabilityDto.EndTime;

            var existingAvailability = await _dbContext.Doctoravailability
                .FirstOrDefaultAsync(a => a.DoctorId == availabilityDto.DoctorId && a.Date == availabilityDto.Date);

            if (existingAvailability == null)
            {
                existingAvailability = new Doctoravailability
                {
                    DoctorId = availabilityDto.DoctorId,
                    Date = availabilityDto.Date,
                    DayOfWeek = availabilityDto.DayOfWeek,
                    StartTime = startTimeOnly,
                    EndTime = endTimeOnly
                };

                await _dbContext.Doctoravailability.AddAsync(existingAvailability);
                await _dbContext.SaveChangesAsync();
            }

            
            var slots = GenerateSlots(startTimeOnly, endTimeOnly, availabilityDto.SlotDurationMinutes);


            var existingSlots = await _dbContext.Doctoravailabilityslots
                .Where(s => s.AvailabilityId == existingAvailability.AvailabilityId)
                .ToListAsync();

            _dbContext.Doctoravailabilityslots.RemoveRange(existingSlots);

            foreach (var slot in slots)
            {
                var newSlot = new Doctoravailabilityslot
                {
                    AvailabilityId = existingAvailability.AvailabilityId,
                    SlotStartTime = slot.Item1,
                    SlotEndTime = slot.Item2
                };

                await _dbContext.Doctoravailabilityslots.AddAsync(newSlot);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        private static List<(TimeOnly, TimeOnly)> GenerateSlots(TimeOnly startTime, TimeOnly endTime, int slotDurationMinutes)
        {
            var slots = new List<(TimeOnly, TimeOnly)>();

            var currentTime = startTime;
            while (currentTime.AddMinutes(slotDurationMinutes) <= endTime)
            {
                var nextTime = currentTime.AddMinutes(slotDurationMinutes);
                slots.Add((currentTime, nextTime));
                currentTime = nextTime;
            }

            return slots;
        }

        public async Task<bool> VerifyDoctorProfile(VerifyDoctorDTO verifyDoctorDto)
        {
            var doctor = await _dbContext.Doctors.FindAsync(verifyDoctorDto.DoctorId);
            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            if (verifyDoctorDto.Status != "Verified" && verifyDoctorDto.Status != "Rejected")
            {
                throw new ArgumentException("Invalid verification status.");
            }

            doctor.VerificationStatus = verifyDoctorDto.Status;

            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
