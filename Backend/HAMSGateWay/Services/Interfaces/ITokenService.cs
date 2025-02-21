using HAMSGateWay.Models;

namespace HAMSGateWay.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string mailId, string role, int userId);
        bool VerifyPassword(string password, string hash, string salt);
        (string passwordHash, string passwordSalt) CreatePasswordHash(string password);
    }
}
