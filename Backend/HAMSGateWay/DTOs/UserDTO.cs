namespace UserService.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        //public string PasswordSalt { get; set; }

        public string Role { get; set; }
    }
}
