using System.ComponentModel.DataAnnotations;

namespace HAMSMicroservices.DTOs
{
    public class AppointmentCancelDTO
    {
        public int AppointmentId { get; set; }
        
        [Required]
        public string Reason { get; set; }
    }
}
