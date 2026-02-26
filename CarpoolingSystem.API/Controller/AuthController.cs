using Microsoft.AspNetCore.Mvc;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;

namespace CarpoolingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            try
            {
                await _authService.RegisterAsync(registerRequest);
                return Ok(new { 
                    Message = "User registered successfully." 
                });
            }
            catch (Exception exception)
            {
                return BadRequest(new {
                    Error = exception.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var jwtToken = await _authService.LoginAsync(loginRequest);
                return Ok(new { 
                    Token = jwtToken 
                });
            }
            catch (Exception exception)
            {
                return Unauthorized(new { 
                    Error = exception.Message 
                });
            }
        }
    }
}