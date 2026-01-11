using BCrypt.Net;
using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Services;
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
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IUserRepository userRepository, IJwtTokenService jwtTokenService )
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!user.IsApproved)
                return Unauthorized("Account awaiting admin approval");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new { token });
        }

    }
}
