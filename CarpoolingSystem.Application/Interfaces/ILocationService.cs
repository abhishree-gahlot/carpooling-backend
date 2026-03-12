using CarpoolingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces {
    public interface ILocationService {

        Task UpdateLocationAsync(Guid userId, UpdateLocationDto dto);
        //Task<IEnumerable<NearbyDriverDto>> GetNearbyDriversAsync(double latitude, double longitude, double radiusKm);
    }
}
