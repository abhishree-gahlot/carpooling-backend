using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Helper {
    public static class DistanceHelper {
        public static double ConvertDegreesToRadians(double degrees) {
            return degrees * Math.PI / 180;
        }

        public static double CalculateDistanceKm(double passengerLatitude, double passengerLongitude, double driverLatitude, double driverLongitude) {
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
