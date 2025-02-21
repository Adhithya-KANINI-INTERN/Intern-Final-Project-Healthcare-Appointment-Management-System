using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HAMSGateWay.Models;
using HAMSGateWay.Services.Interfaces;

namespace HAMSGateWay.Services
{
    public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key;


        public TokenService(IConfiguration configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTToken"]));
        }


        public string GenerateToken(string mailId, string role, int userId)
        {
            var claims = new List<Claim>() {
                new(JwtRegisteredClaimNames.NameId, userId.ToString()),
                new(ClaimTypes.Name, mailId),  
                new(ClaimTypes.Role, role)
            };


            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDesc);
            return tokenHandler.WriteToken(token);
        }

        public (string passwordHash, string passwordSalt) CreatePasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256();
            return (Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))),
                Convert.ToBase64String(hmac.Key));
        }
        public bool VerifyPassword(string password, string hash, string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(salt));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return hash == computedHash;
        }
    }
}
