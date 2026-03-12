using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Services {
    public class LocationService : ILocationService {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository) {
            _locationRepository = locationRepository;
        }
        //--------------------------------------------------------------------------------

        //public Task<IEnumerable<NearbyDriverDto>> GetNearbyDriversAsync(double latitude, double longitude, double radiusKm) {
        //    //Code can be implemented after Sessions related codework is done
        //    throw new NotImplementedException();
        //}

        //--------------------------------------------------------------------------------

            public async Task UpdateLocationAsync(Guid userId, UpdateLocationDto dto) {
            var location = await _locationRepository.GetByUserIdAsync(userId);

            if (location == null) {
                var newLocation = new Location {
                    UserId = userId,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    UpdatedAt = DateTime.UtcNow
                };
                await _locationRepository.AddAsync(newLocation);
            }
            else {
                location.Latitude = dto.Latitude;
                location.Longitude = dto.Longitude;
                location.UpdatedAt = DateTime.UtcNow;
                _locationRepository.Update(location);
            }

            await _locationRepository.SaveChangesAsync();
        }

        //HaversineDistance Formula to be implemented here
    }
}
