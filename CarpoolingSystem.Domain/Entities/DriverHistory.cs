using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Entities {
    public class DriverHistory {

        public Guid DriverHistoryId { get; set; }
        public Guid RideSessionId { get; set; }
        public RideSession RideSession { get; set; } = null!;
        public Guid DriverId { get; set; }
        public User Driver { get; set; } = null!;
        public string StartingLocation { get; set; } = string.Empty;
        public string DestinationLocation { get; set; } = string.Empty;
        public DateTime DateAndTime { get; set; } = DateTime.UtcNow;
        public DateTime? DropOffTime { get; set; }
        public decimal? TotalFare { get; set; }
        public ICollection<DriverHistoryPassenger> Passengers { get; set; }=new List<DriverHistoryPassenger>();


    }
}
