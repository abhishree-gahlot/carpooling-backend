using CarpoolingSystem.Application.DTOs;
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

        public async Task<IEnumerable<NearbyDriverDto>> GetNearbyDrivers(double latitude, double longitude, double radiusMeters)
        {
            var allDriverLocations = _driverLocationStore.GetAllLocations();
            var radiusKm = radiusMeters / 1000.0;

            Console.WriteLine($"[NearbyDrivers] Total drivers in store: {allDriverLocations.Count()}");
            Console.WriteLine($"[NearbyDrivers] Searching | Passenger: {latitude},{longitude} | Radius: {radiusMeters}m");
            Console.WriteLine($"[NearbyDrivers] Total drivers in store: {allDriverLocations.Count()}");

            foreach (var d in allDriverLocations)
            {
                var dist = CalculateDistanceKm(latitude, longitude, d.Latitude, d.Longitude);
                Console.WriteLine($"[NearbyDrivers] Driver {d.DriverId} | Location: {d.Latitude},{d.Longitude} | Distance: {dist}km");
            }

            var nearbyDriverLocations = allDriverLocations
                .Where(driver => CalculateDistanceKm(latitude, longitude, driver.Latitude, driver.Longitude) <= radiusKm)
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

                result.Add(new NearbyDriverDto
                {
                    DriverId = driverLocation.DriverId,
                    SessionId = activeSession.SessionId,
                    DriverName = activeSession.Driver.UserName,
                    VehicleName = activeSession.Vehicle.VehicleName,
                    LicensePlate = activeSession.Vehicle.LicensePlate,
                    AvailableSeats = activeSession.AvailableSeats,
                    Latitude = driverLocation.Latitude,
                    Longitude = driverLocation.Longitude,
                    DistanceKm = Math.Round(CalculateDistanceKm(latitude, longitude, driverLocation.Latitude, driverLocation.Longitude), 2),
                    LastUpdated = driverLocation.UpdatedAt
                });
            }

            return result;
        }

        private double ConvertDegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private double CalculateDistanceKm(double passengerLatitude, double passengerLongitude, double driverLatitude, double driverLongitude)
        {
            const double EarthRadiusKm = 6371;
            var deltaLatitude = ConvertDegreesToRadians(driverLatitude - passengerLatitude);
            var deltaLongitude = ConvertDegreesToRadians(driverLongitude - passengerLongitude);

            var haversineValue = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) +
                    Math.Cos(ConvertDegreesToRadians(passengerLatitude)) * Math.Cos(ConvertDegreesToRadians(driverLatitude)) *
                    Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2);

            var centralAngleRadians = 2 * Math.Atan2(Math.Sqrt(haversineValue), Math.Sqrt(1 - haversineValue));

            return EarthRadiusKm * centralAngleRadians;
        }
    }
}
