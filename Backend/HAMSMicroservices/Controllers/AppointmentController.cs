using HAMSMicroservices.DTOs;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<AppointmentController>
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int doctorId)
        {
            var appointments = await _appointmentService.GetAppointmentsByDoctor(doctorId);

            if(appointments == null || !appointments.Any())
            {
                return NotFound("No Appointments found for this doctor.");
            }

            return Ok(appointments);
        }

        // GET api/<AppointmentController>/5
        [HttpGet("patients/{patientId}")]
        public async Task<IActionResult> GetAppointmentByPatient(int patientId)
        {
            var appointments = await _appointmentService.GetAppointmentsByPatient(patientId);

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No Appointments found for this patient.");
            }

            return Ok(appointments);
        }

        // POST api/<AppointmentController>
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDTO createDto)
        {
            var isCreated = await _appointmentService.CreateAppointment(createDto);

            if(!isCreated)
            {
                return BadRequest("Doctor is not available at the selected time.");
            }

            return Ok("Appointment created successfully.");

        }

        // PUT api/appointments/update
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

        // PUT api/appointments/cancel
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

        [Authorize(Roles = "Doctor")]
        [HttpPut("approve/{appointmentId}")]
        public async Task<IActionResult> ApproveAppointment(int appointmentId, [FromBody] AppointmentApprovalDTO approvalDto)
        {
            var success = await _appointmentService.ApproveAppointment(appointmentId, approvalDto);
            if (!success)
            {
                return BadRequest("Unable to approve the appointment.");
            }
            return Ok("Appointment approved successfully.");
        }

    }
}
