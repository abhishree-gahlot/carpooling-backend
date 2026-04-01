using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Helper;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Repositories;

namespace CarpoolingSystem.Application.Services
{
    public class LocationService: ILocationService
    {
        private readonly IDriverLocationStoreService _driverLocationStore;
        private readonly IRideSessionRepository _rideSessionRepository;

        public LocationService(IDriverLocationStoreService driverLocationStore, IRideSessionRepository rideSessionRepository)
        {
            _driverLocationStore = driverLocationStore;
            _rideSessionRepository = rideSessionRepository;
        }

        public void UpdateDriverLocation(Guid driverId, double latitude, double longitude)
        {
            _driverLocationStore.UpdateLocation(driverId, latitude, longitude);
        }

        public async Task<IEnumerable<NearbyDriverDto>> GetNearbyDrivers(
            double latitude, double longitude,
            double destinationLatitude,
            double destinationLongitude,
            double radiusMeters
        ) {
            var allDriverLocations = _driverLocationStore.GetAllLocations();
            var radiusKm = radiusMeters / 1000.0;
            const double destinationToleranceKm = 0.5;

            var nearbyDriverLocations = allDriverLocations
                .Where(driver => DistanceHelper
                    .CalculateDistanceKm(latitude, longitude, driver.Latitude, driver.Longitude) <= radiusKm)
                .ToList();

            if(!nearbyDriverLocations.Any())
            {
                return Enumerable.Empty<NearbyDriverDto>();
            }

            var result = new List<NearbyDriverDto>();

            foreach (var driverLocation in nearbyDriverLocations)
            {
                var activeSession = await _rideSessionRepository
                    .GetActiveSessionByDriverIdAsync(driverLocation.DriverId);

                if (activeSession == null || activeSession.AvailableSeats <= 0)
                {
                    Console.WriteLine("Driver has no active session");
                    continue; 
                }

                double destinationDistance = DistanceHelper.CalculateDistanceKm(
                    destinationLatitude, destinationLongitude,
                    activeSession.DestinationLatitude,
                    activeSession.DestinationLongitude
                );

                if (destinationDistance > destinationToleranceKm)
                {
                    continue;
                }
                    
                result.Add(new NearbyDriverDto
                {
                    DriverId = driverLocation.DriverId,
                    SessionId = activeSession.Id,
                    DriverName = activeSession.Driver.UserName,
                    VehicleName = activeSession.Vehicle.VehicleName,
                    LicensePlate = activeSession.Vehicle.LicensePlate,
                    AvailableSeats = activeSession.AvailableSeats,
                    Latitude = driverLocation.Latitude,
                    Longitude = driverLocation.Longitude,
                    DistanceKm = Math.Round(DistanceHelper.CalculateDistanceKm(
                                        latitude, longitude, driverLocation.Latitude, driverLocation.Longitude),
                                 2),
                    LastUpdated = driverLocation.UpdatedAt
                });
            }

            return result;
        }
    }
}
