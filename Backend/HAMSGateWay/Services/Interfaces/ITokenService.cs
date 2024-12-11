using UserService.Models;

namespace UserService.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string mailId, string role);
        bool VerifyPassword(string password, string hash, string salt);
        (string passwordHash, string passwordSalt) CreatePasswordHash(string password);

        //Task<string> Login(LoginDTO loginDto);
        //Task<User> Register(RegisterDTO registerDto);
    }
}
