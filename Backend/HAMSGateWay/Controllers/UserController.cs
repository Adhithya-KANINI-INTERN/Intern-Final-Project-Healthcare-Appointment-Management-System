using HAMSGateWay.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HAMSGateWay.Models;
using HAMSGateWay.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HAMSGateWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("total-users")]
        public async Task<IActionResult> GetTotalUsers()
        {
            var totalUsers = await _userService.GetTotalUsers();
            return Ok(totalUsers);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("total-patients")]
        public async Task<IActionResult> GetTotalPatients()
        {
            var totalPatients = await _userService.GetTotalPatients();
            return Ok(totalPatients);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("total-doctors")]
        public async Task<IActionResult> GetTotalDoctors()
        {
            var totalDoctors = await _userService.GetTotalDoctors();
            return Ok(totalDoctors);
        }

        // GET: api/<AuthController>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAllUsers();
            if (users == null)
            {
                return NotFound("No users found");
            }

            return Ok(users);
        }


        [Authorize]
        [HttpGet("userId")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {

            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound($"No user found with email: {userId}");
            }
                
            return Ok(user);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("role/{role}")]
        public async Task<ActionResult<List<User>>> GetUserByRole(string role)
        {
            var user = await _userService.GetUserByRole(role);
            if (user == null)
            {
                return NotFound($"No user found with email: {role}");
            }
            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            bool result = await _userService.Register(registerDto);
            if (result)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("User to register customer");
            }
        }

        // POST api/<AuthController>
        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _userService.Login(loginDto);
            if (result != null && !string.IsNullOrWhiteSpace(result.JWTTokenKey))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Username or Password is incorrect");
            }
        }

        // PUT api/<AuthController>/5
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedUser = await _userService.UpdateUser(userDto);
            if (updatedUser == null)
            {
                return NotFound($"User with ID {userDto.UserId} not found.");
            }

            return Ok("User updated successfully.");
        }

        // UserController.cs
        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ChangePassword(changePasswordDto);
            if (!result)
            {
                return BadRequest("Failed to change password. Please check your current password and try again.");
            }

            return Ok("Password changed successfully.");
        }


        // DELETE api/<AuthController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{email}/delete")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            //if (string.IsNullOrEmpty(email))
            //    return BadRequest("Email is required");

            var result = await _userService.DeleteUser(email);
            if (!result)
                return NotFound($"No user found with email: {email}");

            return Ok("User deleted successfully");
        }
    }
}
