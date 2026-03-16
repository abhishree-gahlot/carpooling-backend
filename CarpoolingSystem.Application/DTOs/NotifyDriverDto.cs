using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs
{
    public class NotifyDriverDto
    {
        public Guid DriverId { get; set; }
        public Guid RideRequestId { get; set; }
        public string PickupName { get; set; } = string.Empty;
        public double PickupLat { get; set; }
        public double PickupLng { get; set; }
        public string DestinationName { get; set; } = string.Empty;
    }
}
