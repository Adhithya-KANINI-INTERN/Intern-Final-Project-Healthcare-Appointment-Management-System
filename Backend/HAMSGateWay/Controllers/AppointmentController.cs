using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HAMSGateway.DTOs;
using HAMSGateWay.Models;
using HAMSGateWay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HAMSGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("all-appointments")]

        public async Task<IActionResult> GetAllAppointments()
        {
            var allAppointments = await _appointmentService.GetAllAppointments();

            return Ok(allAppointments);
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("total-appointments")]
        public async Task<IActionResult> GetTotalConfirmedAppointments()
        {
            var totalConfirmedAppointments = await _appointmentService.GetTotalConfirmedAppointments();

            return Ok(totalConfirmedAppointments);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("doctor-appointments/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            var appointments = await _appointmentService.GetAppointmentsByDoctor(doctorId);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found for this doctor.");
            }

            return Ok(appointments);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("by-doctor/{userId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int userId)
        {
            var appointments = await _appointmentService.GetAppointmentsByDoctor(userId);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found for you.");
            }

            return Ok(appointments);
        }


        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatient(int patientId)
        {
            var appointments = await _appointmentService.GetAppointmentsByPatient(patientId);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found for this patient.");
            }

            return Ok(appointments);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("patient-appointment")]
        public async Task<IActionResult> GetAppointmentsPatient()
        {
            var appointments = await _appointmentService.GetAppointmentsPatient();
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments found for this patient.");
            }

            return Ok(appointments);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDTO createDto)
        {
            var success = await _appointmentService.CreateAppointment(createDto);
            if (!success)
            {
                return BadRequest("Failed to create the appointment.");
            }

            return Ok("Appointment created successfully.");
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPut("update/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentUpdateDTO updateDto)
        {
            var success = await _appointmentService.UpdateAppointment(updateDto);
            if (!success)
            {
                return NotFound("Appointment not found or could not update.");
            }

            return Ok("Appointment updated successfully.");
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpPut("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment([FromBody] AppointmentCancelDTO cancelDto)
        {
            var success = await _appointmentService.CancelAppointment(cancelDto);
            if (!success)
            {
                return NotFound("Appointment not found or could not cancel.");
            }

            return Ok("Appointment canceled successfully.");
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("completed/{appointmentId}")]
        public async Task<IActionResult> CancelAppointmentByDoctor(int appointmentId)
        {
            var isMarkComleted = await _appointmentService.MarkAppointmentAsCompleted(appointmentId);
            if (!isMarkComleted)
            {
                return BadRequest("Appointment could not be mark as completed.");
            }
            return Ok("Appointment marked as completed.");
        }
    }
}
