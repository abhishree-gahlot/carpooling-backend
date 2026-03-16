using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarpoolingSystem.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("nearbyDrivers")]
        public async Task<IActionResult> GetNearbyDrivers(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] double radius = 2000
        )
        {
            var nearbyDrivers = await _locationService.GetNearbyDrivers(latitude, longitude, radius);

            if (!nearbyDrivers.Any())
            {
                return NotFound(new
                {
                    message = "No drivers found nearby."
                });
            }

            return Ok(new
            {
                drivers = nearbyDrivers
            });
        }

        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateDriverLocation(
            [FromBody] UpdateLocationDto updateLocationDto
        )
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            Guid driverId = Guid.Parse(userIdClaim);
            _locationService.UpdateDriverLocation(driverId, updateLocationDto.Latitude, updateLocationDto.Longitude);

            return Ok(new
            {
                message = "Driver location updated successfully."
            });
        }
    }
}
