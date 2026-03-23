using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarpoolingSystem.Application.Interfaces;
using System.Security.Claims;

namespace CarpoolingSystem.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController: ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("my-pin")]
        public async Task<IActionResult> GetMyPin()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userIdClaim) )
            {
                return Unauthorized();
            }

            var user = await _authService.GetUserByIdAsync(Guid.Parse(userIdClaim));

            if ((user == null)) 
            {
                return NotFound();
            }

            return Ok(new
            {
                pin = user.Pin
            });
        }
    }
}
