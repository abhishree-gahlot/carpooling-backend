using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Entities {
    public class RideRequests {

        public Guid Id { get; set; }
        public Guid PassengerId { get; set; }
        public User Passenger { get; set; } = null!;
        public string Pickup { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
        public RideRequestStatus RideRequestStatus { get; set; }
            = RideRequestStatus.Pending;

    }
}
