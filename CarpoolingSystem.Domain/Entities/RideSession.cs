using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Entities {
    public class RideSession {

        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public User Driver { get; set; } = null!;
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public Guid? PassengerId { get; set; }
        public User? Passenger { get; set; }
        public string Pickup {  get; set; } = string.Empty;
        public string Dropoff { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartedAt { get; set; }= DateTime.UtcNow;
        public DateTime? EndedAt { get; set; }

    }
}
