using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarpoolingSystem.API.Controller {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService) {
            _locationService=locationService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLocation(UpdateLocationDto dto) {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            await _locationService.UpdateLocationAsync(Guid.Parse(userIdClaim), dto);
            return Ok(new { message = "Location updated successfully." });
        }

        //[HttpGet("nearby")]
        //public async Task<IActionResult> GetNearbyDrivers([FromQuery] double latitude,[FromQuery] double longitude,[FromQuery] double radiusKm = 2.0) {
        //    var drivers = await _locationService.GetNearbyDriversAsync(latitude, longitude, radiusKm);
        //    return Ok(drivers);
        //}
﻿using Microsoft.AspNetCore.Mvc;
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
