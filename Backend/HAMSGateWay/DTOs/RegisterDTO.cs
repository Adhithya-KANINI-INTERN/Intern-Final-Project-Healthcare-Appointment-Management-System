using System.ComponentModel.DataAnnotations;

namespace UserService.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string FullName { get; set; } = null!;
        [Required]
        [MinLength(8)]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!; 
    }
}
