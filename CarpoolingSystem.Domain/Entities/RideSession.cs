using CarpoolingSystem.Domain.Enums;
using System;

namespace CarpoolingSystem.Domain.Entities
{
    public class RideSession
    {

        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public User Driver { get; set; } = null!;
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string PickupName { get; set; } = string.Empty;
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public string DestinationName { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
        public RideSessionStatus Status { get; set; } = RideSessionStatus.Waiting;
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAt { get; set; }
        public DriverAvailability DriverAvailability { get; set; } = DriverAvailability.Available;
    }
}