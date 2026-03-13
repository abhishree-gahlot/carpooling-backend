using CarpoolingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface IReverseGeocodingService
    {
        Task<LocationDto?> GetLocationAsync(double latitude, double longitude);
    }
}
