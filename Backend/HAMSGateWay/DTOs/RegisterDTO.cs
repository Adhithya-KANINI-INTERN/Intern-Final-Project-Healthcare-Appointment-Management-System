using System.ComponentModel.DataAnnotations;

namespace HAMSGateWay.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!; 
    }
}
