using System;
using System.Collections.Generic;
using System.Text;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;

namespace CarpoolingSystem.Application.Services
{
    public class LocationService
    {
        public readonly IReverseGeocodingService _reverseGeoCodingService;

        public LocationService(IReverseGeocodingService reverseGeoCodingService)
        {
            _reverseGeoCodingService = reverseGeoCodingService;
        }

        public async Task<LocationDto?> GetLocationFromCoordinatesAsync(double latitude, double longitude)
        {
            return await _reverseGeoCodingService.GetLocationAsync(latitude, longitude);
        }
    }
}
