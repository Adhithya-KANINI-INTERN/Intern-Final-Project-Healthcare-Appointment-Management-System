using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Models;
using UserService.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.Controllers
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

        // GET: api/<AuthController>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _userService.GetAllUsers();
            if(users == null)
            {
                return NotFound("No users found");
            }

            return Ok(users);
        }


        [HttpGet("email")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUserByEmail(UserDTO userDto)
        {
            if (string.IsNullOrEmpty(userDto.Email))
                return BadRequest("Email is required");

            var user = await _userService.GetUserByEmail(userDto);
            if (user == null)
                return NotFound($"No user found with email: {userDto.Email}");

            return Ok(user);
        }


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

        // POST api/<AuthController>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            bool result = await _userService.Register(registerDto);
            if (result)
            {
                return Ok("Customer registered successfully");
            }
            else
            {
                return BadRequest("Unable to register customer");
            }
        }

        // POST api/<AuthController>
        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _userService.Login(loginDto);
            if (result != null && !String.IsNullOrWhiteSpace(result.JWTTokenKey))
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
        [Authorize(Roles = "Admin")]
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

        // DELETE api/<AuthController>/5
        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required");

            var result = await _userService.DeleteUser(email);
            if (!result)
                return NotFound($"No user found with email: {email}");

            return Ok("User deleted successfully");
        }
    }
}
