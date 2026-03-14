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
        //public Guid? PassengerId { get; set; }
        //public string? PassengerName { get; set; }
        public string Pickup { get; set; } = string.Empty;
        public string Dropoff { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
