﻿using HAMSMicroservices.DTOs;
using HAMSMicroservices.Models;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HAMSMicroservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {

        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        [HttpGet("check-profile/{userId}")]
        public async Task<IActionResult> CheckDoctorProfile(int userId)
        {
            var doctor = await _doctorService.CheckVerifiedDoctorProfile(userId);
            if(!doctor)
            {
                return NotFound("Doctor profile not found.");
            }

            return Ok("Doctor Profile Found");
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetDoctorProfilebyId(int doctorId)
        {
            var doctorProfile = await _doctorService.GetDoctorProfilebyId(doctorId);
            if (doctorProfile == null)
            {
                return NotFound($"Doctor with ID {doctorId} does not exist.");
            }

            return Ok(doctorProfile);

        }

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetDoctorProfile(int userId)
        {
            var doctorProfile = await _doctorService.GetDoctorProfile(userId);
            if (doctorProfile == null)
            {
                return NotFound($"Doctor does not exist.");
            }

            return Ok(doctorProfile);

        }

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

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingDoctors()
        {
            try
            {
                var pendingDoctors = await _doctorService.GetPendingDoctorsAsync();

                if (pendingDoctors == null || !pendingDoctors.Any())
                {
                    return NotFound(new { message = "No pending doctors found." });
                }

                return Ok(pendingDoctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving pending doctors.", details = ex.Message });
            }
        }

        [HttpPost("add-profile")]
        public async Task<IActionResult> AddDoctorProfile([FromBody] AddDoctorProfileDTO dto)
        {
            try
            {
                await _doctorService.AddDoctorProfile(dto.UserId, dto.Specialization, dto.YearsOfExperience);
                return Ok("Doctor profile added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // PUT api/<DoctorController>/5
        [HttpPut("updateprofile")]
        public async Task<IActionResult> UpdateDoctor([FromBody] DoctorDTO doctorDTO)
        {

            var result = await _doctorService.UpdateDoctor(doctorDTO);
            if(!result)
            {
                return NotFound("Doctor profile not found for update.");
            }

            return Ok("Doctor profile upated successfully.");
        }


        [HttpGet("{doctorId}/doctor-slots")]
        public async Task<ActionResult> GetDoctorSlots(int doctorId, [FromQuery(Name = "appointmentDate")] DateTime date)
        {
            var availableSlots = await _doctorService.GetDoctorSlots(doctorId, date);
            if (availableSlots == null || availableSlots.Count == 0)
            {
                return NotFound("No available slots found for the doctor on the selected date.");
            }

            return Ok(availableSlots);
        }

        [HttpGet("{doctorId}/available-slots")]
        public async Task<IActionResult> GetAvailableSlotsofDoctor(int doctorId, [FromQuery] DateTime appointmentDate)
        {
            Console.WriteLine($"Received request for doctorId: {doctorId}, appointmentDate: {appointmentDate}");
            var availableSlots = await _doctorService.GetAvailableSlotsofDoctor(doctorId, appointmentDate);

            if (availableSlots == null || availableSlots.Count == 0)
            {
                return NotFound("No available slots found for the doctor on the selected date.");
            }

            return Ok(availableSlots);
        }



        // POST api/<DoctorController>
        [HttpPost("availability")]
        public async Task<IActionResult> ManageAvailability([FromBody] DoctorAvailabilityDTO doctorAvailabilityDto)
        {

            var result = await _doctorService.ManageAvailability(doctorAvailabilityDto);

            if(!result)
            {
                return BadRequest("Failed to manage availability.");
            }

            return Ok("Availability managed successfully.");
        }

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
