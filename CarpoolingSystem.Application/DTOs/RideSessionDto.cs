using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideSessionDto {
        public Guid RideId { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; } = string.Empty;
        public LocationDto Pickup { get; set; } = new();
        public LocationDto Destination { get; set; } = new();
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
