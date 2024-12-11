using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models;
using UserService.Services.Interfaces;

namespace UserService.Services
{
    public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTToken"]));
        }

        public string GenerateToken(string mailId, string role)
        {
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.NameId, mailId),
                new Claim(ClaimTypes.Role, role)
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



        //private readonly UserDbContext _dbContext;
        //private readonly IConfiguration _configuration;

        //public TokenService(UserDbContext context, IConfiguration configuration)
        //{
        //    _dbContext = context;
        //    _configuration = configuration;
        //}

        //public async Task<string> Login(LoginDTO loginDto)
        //{
        //    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    return GenerateToken(user);
        //}

        //public async Task<User> Register(RegisterDTO registerDto)
        //{
        //    if(await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
        //    {
        //        return null;
        //    }

        //    var (passwordHash, passwordSalt) = CreatePasswordHash(registerDto.Password);

        //    var newUser = new User
        //    {
        //        FullName = registerDto.FullName,
        //        Email = registerDto.Email,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt,
        //        Role = registerDto.Role
        //    };

        //    _dbContext.Users.Add(newUser);
        //    await _dbContext.SaveChangesAsync();
        //    return newUser;
        //}

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

        //private string GenerateToken(User user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.Email),
        //        new Claim(ClaimTypes.Role, user.Role)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken"]));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
