using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class BookingCreateDto {
        public Guid RideRequestId { get; set; }
        public Guid SessionId { get; set; }
    }
}
