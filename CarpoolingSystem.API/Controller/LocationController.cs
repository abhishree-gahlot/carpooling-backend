using Microsoft.AspNetCore.Mvc;
using CarpoolingSystem.Application.Services;

namespace CarpoolingSystem.API.Controller
{
    [ApiController]
    [Route("api/location")]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {   
            _locationService = locationService;
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetLocation(double latitude, double longitude)
        {
            var location = await _locationService.GetLocationFromCoordinatesAsync(latitude, longitude);
            return Ok(new { location });
        }
    }
}
