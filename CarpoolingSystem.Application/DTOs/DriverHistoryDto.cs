using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class DriverHistoryDto {
        public Guid DriverHistoryId { get; set; }
        public Guid RideSessionId { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string StartingLocation { get; set; } = string.Empty;
        public string DestinationLocation { get; set; } = string.Empty;
        public DateTime DateAndTime { get; set; } = DateTime.UtcNow;
        public DateTime? DropOffTime { get; set; }
        public decimal? TotalFare { get; set; }
        public List<DriverHistoryPassengerDto> Passengers { get; set; } = new();
    }
}
