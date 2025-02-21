
using HAMSGateWay.DTOs;
using HAMSGateWay.Models;
using System.Threading.Tasks;

namespace HAMSGateWay.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int userId);
        Task<List<User>> GetUserByRole(string role);
        Task<LoginDTO> Login(LoginDTO loginDto);
        Task<bool> Register(RegisterDTO registerDto);
        Task<User> UpdateUser(UserDTO userDto);
        Task<bool> ChangePassword(ChangePasswordDTO changePasswordDto);
        Task<bool> DeleteUser(string email);
        Task<int> GetTotalUsers();
        Task<int> GetTotalDoctors();
        Task<int> GetTotalPatients();
    }
}
