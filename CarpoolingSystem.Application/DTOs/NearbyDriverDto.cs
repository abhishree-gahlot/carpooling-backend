using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    internal class NearbyDriverDto {

        public Guid DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double DistanceKm { get; set; }
    }
}
