using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Enums {
    public enum BookingStatus {
        Pending = 1,
        Accepted = 2,
        Boarded = 3,
        Completed = 4,
        Rejected = 5,
        Cancelled = 6
    }
}
