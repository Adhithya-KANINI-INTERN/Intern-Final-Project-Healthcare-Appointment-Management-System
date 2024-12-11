using HAMSMicroservices.DTOs;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        // GET api/<DoctorController>/5
        [HttpGet("profile")]
        public async Task<IActionResult> GetDoctorProfile()
        {
            var doctorId = _doctorService.GetDoctorIdFromToken();
            var doctorProfile = await _doctorService.GetDoctorProfile(doctorId);

            if (doctorProfile == null)
            {
                return NotFound("Doctor profile not found.");
            }

            return Ok(doctorProfile);

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



        // PUT api/<DoctorController>/5
        [HttpPut("updateprofile")]
        public async Task<IActionResult> UpdateDoctor([FromBody] DoctorDTO doctorDTO)
        {
            var doctorId = _doctorService.GetDoctorIdFromToken();
            doctorDTO.DoctorId = doctorId;

            var result = await _doctorService.UpdateDoctor(doctorDTO);
            if(!result)
            {
                return NotFound("Doctor profile not found for update.");
            }

            return Ok("Doctor profile upated successfully.");
        }

        [HttpGet("doctor/{doctorId}/available-slots")]
        public async Task<IActionResult> GetAvailableSlots(int doctorId, [FromQuery] DateTime appointmentDate)
        {
            var availableSlots = await _doctorService.GetAvailableSlotsForDoctor(doctorId, appointmentDate);

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
            var doctorId = _doctorService.GetDoctorIdFromToken();
            doctorAvailabilityDto.DoctorId = doctorId;

            var result = await _doctorService.ManageAvailability(doctorAvailabilityDto);

            if(!result)
            {
                return BadRequest("Failed to manage availability.");
            }

            return Ok("Availability managed successfully.");
        }

        //[HttpPut("appointmets/{appointmentId}/status")]

        //public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, [FromBody] AppointmentStatusDTO statusDto)
        //{
        //    //var doctorId = _doctorService.GetDoctorIdFromToken();
        //    statusDto.AppointmentId = appointmentId;

        //    var result = await _doctorService.UpdateAppointmentStatus(appointmentId, statusDto);

        //    if(!result)
        //    {
        //        return NotFound("Appointment not found or could not update status.");
        //    }

        //    return Ok("Appointment status updated successfully.");
        //}
    }
}
