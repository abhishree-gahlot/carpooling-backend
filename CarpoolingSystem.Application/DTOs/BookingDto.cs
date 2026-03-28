using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class BookingDto {

        public Guid BookingId { get; set; }
        public Guid SessionId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public string PassengerName { get; set; } = string.Empty;
        public List<Guid> RideRequestIds { get; set; } = new();
        public List<string> PINs { get; set; } = new();
        public List<decimal> Fares { get; set; } = new();
        public List<BookingStatus> Statuses { get; set; } = new();
        public List<DateTime> CreatedAts { get; set; } = new();
        public List<DateTime?> AcceptedAts { get; set; } = new();
        public List<DateTime?> BoardedAts { get; set; } = new();
        public List<DateTime?> CompletedAts { get; set; } = new();
        public List<DateTime?> EndedAts { get; set; } = new();
    }
}
