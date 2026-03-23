using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Entities {
    public class DriverHistoryPassenger {
        public Guid PassengerHistoryId { get; set; }
        public Guid DriverHistoryId { get; set; }
        public DriverHistory DriverHistory { get; set; } = null!;
        public Guid RideId { get; set; }
        public RideRequests RideRequests { get; set; }= null!;
        public string PassengerName { get; set; }=string.Empty;
        public string Pickup { get; set; }=string.Empty;
        public decimal? Fare { get; set; }
        public DateTime? PickupTime { get; set; }
        public decimal? Ratings { get; set; }


    }
}
