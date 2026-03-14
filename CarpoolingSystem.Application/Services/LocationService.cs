using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Services
{
    public class LocationService: ILocationService
    {
        private readonly IDriverLocationStore _driverLocationStore;

        public LocationService(IDriverLocationStore driverLocationStore)
        {
            _driverLocationStore = driverLocationStore;
        }

        public void UpdateDriverLocation(Guid driverId, double latitude, double longitude)
        {
            _driverLocationStore.UpdateLocation(driverId, latitude, longitude);
        }

        public IEnumerable<NearbyDriverDto> GetNearbyDrivers(double latitude, double longitude, double radiusMeters)
        {
            var allDrivers = _driverLocationStore.GetAllLocations();
            var radiusKm = radiusMeters / 1000.0;

            return allDrivers
                .Where(driver => CalculateDistanceKm(latitude, longitude, driver.Latitude, driver.Longitude) <= radiusKm)
                .Select(driver => new NearbyDriverDto
                {
                    DriverId = driver.DriverId,
                    Latitude = driver.Latitude,
                    Longitude = driver.Longitude,
                    DistanceKm = Math.Round(CalculateDistanceKm(latitude, longitude, driver.Latitude, driver.Longitude), 2),
                    LastUpdated = driver.UpdatedAt
                });
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
