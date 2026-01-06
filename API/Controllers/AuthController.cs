using BCrypt.Net;
using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="userDto">An object containing the user's registration information, including email and password. Cannot be null.</param>
        /// <returns>An IActionResult indicating the result of the registration operation. Returns 200 OK if registration is
        /// successful, 400 Bad Request if the input is null, or 409 Conflict if a user with the specified email already
        /// exists.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null.");
            }
            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                return Conflict("User with this email already exists.");
            }
            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                IsApproved = false,
                Role = "User",
                CreatedOn = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return Ok("User registered successfully.");
        }
    }
}
