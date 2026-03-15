using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class NearbyDriverDto {

        public Guid DriverId { get; set; }
        public string DriverName { get; set; } = String.Empty;
        public string VehicleName { get; set; } = String.Empty;
        public string LicensePlate { get; set; } = String.Empty;
        public int AvailableSeats { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceKm { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
