using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HAMSGateway.DTOs;
using HAMSGateWay.DTOs;
using HAMSGateWay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HAMSGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _doctorService;

        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("check-profile/{userId}")]
        public async Task<IActionResult> CheckDoctorProfile(int userId)
        {
            var doctor = await _doctorService.CheckDoctorProfile(userId);
            if (!doctor)
            {
                return NotFound("Doctor profile not found.");
            }

            return Ok( new { message = "Doctor Profile Found" } );
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetDoctorProfilebyId(int doctorId)
        {
            var doctorProfile = await _doctorService.GetDoctorProfilebyId(doctorId);
            if (doctorProfile == null)
            {
                return NotFound("Doctor profile not found.");
            }

            return Ok(doctorProfile);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetDoctorProfile(int userId)
        {
            var doctorProfile = await _doctorService.GetDoctorProfile(userId);
            if (doctorProfile == null)
            {
                return NotFound("Doctor profile not found.");
            }

            return Ok(doctorProfile);
        }

        [Authorize]
        [HttpGet("all-profile")]
        public async Task<IActionResult> GetAllDoctorProfile()
        {
            var doctorProfiles = await _doctorService.GetAllDoctorProfile();
            if (doctorProfiles == null)
            {
                return NotFound($"Doctor does not exist.");
            }

            return Ok(doctorProfiles);

        }

        [Authorize]
        [HttpGet("specialization/{specialization}")]
        public async Task<IActionResult> GetDoctorsBySpecialization(string specialization)
        {
            var doctors = await _doctorService.GetDoctorsBySpecialization(specialization);
            if (doctors == null || doctors.Count == 0)
            {
                return NotFound("No doctors found with this specialization.");
            }

            return Ok(doctors);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingDoctors()
        {
            var pendingDoctors = await _doctorService.GetPendingDoctors();

            if (pendingDoctors == null)
            {
                return NotFound(new { message = "No pending doctors found." });
            }

            return Ok(pendingDoctors);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("add-profile")]
        public async Task<IActionResult> AddDoctorProfile([FromBody] AddDoctorProfileDTO addDoctorProfileDTO)
        {
            var result = await _doctorService.AddDoctorProfile(addDoctorProfileDTO);
            if (!result)
            {
                return NotFound("Doctor profile not found for add.");
            }

            return Ok("Doctor profile added successfully.");
        }
        
        [Authorize(Roles = "Doctor")]
        [HttpPut("updateprofile")]
        public async Task<IActionResult> UpdateDoctorProfile([FromBody] DoctorDTO doctorDTO)
        {
            var result = await _doctorService.UpdateDoctorProfile(doctorDTO);
            if (!result)
            {
                return NotFound("Doctor profile not found for update.");
            }

            return Ok("Doctor profile updated successfully.");
        }

        [Authorize]
        [HttpGet("{doctorId}/doctor-slots")]
        public async Task<IActionResult> GetDoctorSlots(int doctorId, [FromQuery(Name = "appointmentDate")] DateTime date)
        {
            Console.WriteLine($"Received date: {date}");
            var doctorSlots = await _doctorService.GetDoctorSlots(doctorId, date);
            if (doctorSlots == null || doctorSlots.Count == 0)
            {
                return NotFound("No available slots found for the doctor on the selected date.");
            }

            return Ok(doctorSlots);
        }

        [Authorize]
        [HttpGet("{doctorId}/available-slots")]
        public async Task<IActionResult> GetAvailableSlotsofDoctor(int doctorId, [FromQuery] DateTime appointmentDate)
        {
            var availableSlots = await _doctorService.GetAvailableSlotsofDoctor(doctorId, appointmentDate);
            if (availableSlots == null || availableSlots.Count == 0)
            {
                return NotFound("No available slots found for the doctor on the selected date.");
            }

            return Ok(availableSlots);
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost("availability")]
        public async Task<IActionResult> ManageAvailability([FromBody] DoctorAvailabilityDTO doctorAvailabilityDto)
        {
            
            var result = await _doctorService.ManageDoctorAvailability(doctorAvailabilityDto);
            if (!result)
            {
                return BadRequest("Failed to manage availability.");
            }

            return Ok("Doctor availability managed successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("verify-doctor")]
        public async Task<IActionResult> VerifyDoctorProfile([FromBody] VerifyDoctorDTO verifyDoctorDto)
        {
            var result = await _doctorService.VerifyDoctorProfile(verifyDoctorDto);
            if (!result)
            {
                
                return BadRequest("Failed to verify");
            }

            return Ok(new { message = $"Doctor verification status updated to {verifyDoctorDto.Status}." });
        }
    }
}
