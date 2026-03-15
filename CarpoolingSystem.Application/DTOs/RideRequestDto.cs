using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideRequestDto {
        public Guid RequestId { get; set; }
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public LocationDto Pickup { get; set; } = new();
        public LocationDto Destination { get; set; } = new();
        public DateTime RequestedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public RideRequestStatus RideRequestStatus { get; set; }
    }
}
