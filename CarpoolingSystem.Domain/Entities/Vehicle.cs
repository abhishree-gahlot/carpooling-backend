using System;

namespace CarpoolingSystem.Domain.Entities
{
    public class Vehicle
    {
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; } = string.Empty;
        public Guid DriverId { get; set; }
        public int MaxSeats { get; set; }
        public bool IsActive { get; set; } = true;
        public string LicensePlate { get; set; } = string.Empty;
        public User Driver { get; set; } = null!;
    }
}