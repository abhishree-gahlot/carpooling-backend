using CarpoolingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface ILocationService
    {
        IEnumerable<NearbyDriverDto> GetNearbyDrivers(double latitude, double longitude, double radiusMeters);
        void UpdateDriverLocation(Guid driverId, double latitude, double longitude);
    }
}
