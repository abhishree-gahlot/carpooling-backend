using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class BookingDto {

        public Guid BookingId { get; set; }
        public Guid RideRequestId { get; set; }
        public Guid SessionId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public string PassengerName { get; set; } = string.Empty;
        public string PIN { get; set; } = string.Empty;
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? BoardedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
