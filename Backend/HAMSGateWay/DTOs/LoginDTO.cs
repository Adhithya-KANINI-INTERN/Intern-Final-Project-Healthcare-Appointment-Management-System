namespace UserService.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string JWTTokenKey { get; set; }
    }
}
