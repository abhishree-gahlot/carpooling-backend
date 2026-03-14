using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideRequestDto {
        public Guid RequestId { get; set; }
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string Pickup { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public RideRequestStatus RideRequestStatus { get; set; }
    }
}
