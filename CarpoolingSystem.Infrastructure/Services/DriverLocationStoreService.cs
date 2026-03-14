using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Application.DTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Infrastructure.Services
{
    public class DriverLocationStoreService : IDriverLocationStore
    {
        private readonly ConcurrentDictionary<Guid, DriverLocationDto> _locations = new();

        public void UpdateLocation(Guid driverId, double latitude, double longitude)
        {
            _locations.AddOrUpdate(
                key: driverId,
                addValue: new DriverLocationDto
                {
                    DriverId = driverId,
                    Latitude = latitude,
                    Longitude = longitude,
                    UpdatedAt = DateTime.UtcNow
                },
                updateValueFactory: (key, old) => new DriverLocationDto
                {
                    DriverId = driverId,
                    Latitude = latitude,
                    Longitude = longitude,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
        public IEnumerable<DriverLocationDto> GetAllLocations()
        {
            return _locations.Values;
        }

        public bool TryRemove(Guid driverId)
        {
            return _locations.TryRemove(driverId, out _);
        }
    }
}
