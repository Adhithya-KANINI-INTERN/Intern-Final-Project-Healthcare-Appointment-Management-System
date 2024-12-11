using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserService.DTOs;
using UserService.Models;
using UserService.Services.Interfaces;

namespace UserService.Services
{
    public class UserServices : IUserService
    {

        private readonly UserDbContext _dbContext;

        private readonly ITokenService _tokenService;

        public UserServices(UserDbContext context, ITokenService tokenService )
        {
            _dbContext = context;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                return users;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return null;
            }
        }

        public async Task<User> GetUserByEmail(UserDTO userdto)
        {
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync( x => x.Email == userdto.Email );
                if( user == null )
                {
                    return null;
                }
                return user;
            } 
            catch ( Exception ex )
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<User>> GetUserByRole(string role)
        {
            try
            {
                var users = await _dbContext.Users.Where(x => x.Role == role).ToListAsync();
                if (users == null)
                {
                    return null;
                }
                return users;
            }
            catch ( Exception ex )
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<LoginDTO> Login(LoginDTO loginDto)
        {
            try
            {

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null || !_tokenService.VerifyPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }

                loginDto.JWTTokenKey = _tokenService.GenerateToken(user.Email, user.Role);

                return loginDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
        }

        public async Task<bool> Register(RegisterDTO registerDto)
        {
            bool success = false;
            try
            {
                if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
                {
                    return success;
                }

                var (passwordHash, passwordSalt) = _tokenService.CreatePasswordHash(registerDto.Password);

                var newUser = new User
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = registerDto.Role
                };

                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return success;
            }
            return success;

        }

        public async Task<User> UpdateUser(UserDTO userDto)
        {
            try
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserId == userDto.UserId);
                if (user == null)
                {
                    return null;
                }

                user.FullName = userDto.FullName ?? user.FullName;
                user.Email = userDto.Email ?? user.Email;
                user.Role = userDto.Role ?? user.Role;
                

                if (!string.IsNullOrWhiteSpace(userDto.Password))
                {
                    var (passwordHash, passwordSalt) = _tokenService.CreatePasswordHash(userDto.Password);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> DeleteUser(string email)
        {
            try
            {
                var delete = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (delete == null)
                {
                    return false;
                }

                _dbContext.Users.Remove(delete);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error deleting admin: {ex.Message}");
                return false;
            }
        }

    }
}
