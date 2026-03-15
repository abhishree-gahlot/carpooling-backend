using CarpoolingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarpoolingSystem.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController: ControllerBase
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

            if(!nearbyDrivers.Any())
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
        public IActionResult UpdateDriverLocation(
            [FromQuery] Guid driverId,
            [FromQuery] double latitude,
            [FromQuery] double longitude
        )
        {
            _locationService.UpdateDriverLocation(driverId, latitude, longitude);
            return Ok(new
            {
                message = "Driver location updated successfully."
            });
        }
    }
}
