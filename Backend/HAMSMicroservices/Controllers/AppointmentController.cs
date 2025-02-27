﻿using HAMSMicroservices.DTOs;
using HAMSMicroservices.Models;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HAMSMicroservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("all-appointments")]

        public async Task<ActionResult> GetAllAppointments()
        {
            var allAppointments = await _appointmentService.GetAllAppointments();

            return Ok(allAppointments);
        }


        [HttpGet("total-appointments")]
        public async Task<IActionResult> GetTotalConfirmedAppointments()
        {
            var totalConfirmedAppointments = await _appointmentService.GetTotalConfirmedAppointments();

            return Ok(totalConfirmedAppointments);
        }


        [HttpGet("doctor-appointments/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            var appointments = await _appointmentService.GetAppointmentsByDoctor(doctorId);

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found for this doctor.");
            }

            return Ok(appointments);
        }

        [HttpGet("by-doctor/{userId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int userId)
        {
            var appointments = await _appointmentService.GetAppointmentsByDoctor(userId);

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found.");
            }

            return Ok(appointments);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatient(int patientId)
        {
            var appointments = await _appointmentService.GetAppointmentsByPatient(patientId);

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found for this patient.");
            }

            return Ok(appointments);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDTO createDto)
        {
            var isCreated = await _appointmentService.CreateAppointment(createDto);

            if (!isCreated)
            {
                return BadRequest("Doctor is not available at the selected time.");
            }

            return Ok("Appointment created successfully.");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentUpdateDTO updateDto)
        {
            var isUpdated = await _appointmentService.UpdateAppointment(updateDto);

            if (!isUpdated)
            {
                return BadRequest("Appointment could not be updated.");
            }

            return Ok("Appointment updated successfully.");
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelAppointment([FromBody] AppointmentCancelDTO cancelDto)
        {
            var isCancelled = await _appointmentService.CancelAppointment(cancelDto);

            if (!isCancelled)
            {
                return BadRequest("Appointment could not be cancelled.");
            }

            return Ok("Appointment cancelled successfully.");
        }

        [HttpPut("cancel-by-doctor")]
        public async Task<IActionResult> CancelAppointmentByDoctor([FromBody] AppointmentCancelDTO cancelDto)
        {
            var isCancelled = await _appointmentService.CancelAppointmentByDoctor(cancelDto);

            if (!isCancelled)
            {
                return BadRequest("Appointment could not be cancelled by the doctor.");
            }

            return Ok("Appointment cancelled successfully.");
        }

        [HttpPut("completed/{appointmentId}")]
        public async Task<IActionResult> MarkAppointmentAsCompleted(int appointmentId)
        {
            var isMarkedCompleted = await _appointmentService.MarkAppointmentAsCompleted(appointmentId);

            if (!isMarkedCompleted)
            {
                return BadRequest("Appointment could not be marked as completed.");
            }

            return Ok("Appointment marked as completed.");
        }
    }
}
