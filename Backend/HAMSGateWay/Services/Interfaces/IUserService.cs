
using UserService.DTOs;
using UserService.Models;

namespace UserService.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserByEmail(UserDTO userDto);
        Task<List<User>> GetUserByRole(string role);
        Task<LoginDTO> Login(LoginDTO loginDto);
        Task<bool> Register(RegisterDTO registerDto);
        Task<User> UpdateUser(UserDTO userDto);
        Task<bool> DeleteUser(string email);
    }
}
