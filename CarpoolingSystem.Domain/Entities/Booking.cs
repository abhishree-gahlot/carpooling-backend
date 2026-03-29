using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Entities {
    public class Booking {

        public Guid BookingId { get; set; }
        public Guid RideRequestId { get; set; }
        public RideRequests RideRequest { get; set; } = null!;
        public Guid SessionId { get; set; }
        public RideSession RideSession { get; set; } = null!;
        public string PIN { get; set; } = string.Empty;
        public decimal Fare { get; set; } = 0;
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AcceptedAt { get; set; }
        public DateTime? BoardedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
