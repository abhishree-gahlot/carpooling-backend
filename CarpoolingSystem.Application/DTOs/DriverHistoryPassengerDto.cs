using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class DriverHistoryPassengerDto {
        public Guid PassengerHistoryId { get; set; }
        public Guid DriverHistoryId { get; set; }
        public Guid RideId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string Pickup { get; set; } = string.Empty;
        public decimal? Fare { get; set; }
        public DateTime? PickupTime { get; set; }
        public decimal? Ratings { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
    }
}
