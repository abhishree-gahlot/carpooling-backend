using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Entities {
    public class Booking {

        public Guid BookingId { get; set; }
        public Guid SessionId { get; set; }
        public RideSession RideSession { get; set; } = null!;
        public List<Guid> RideRequestIds { get; set; } = new();
        public List<string> PINs { get; set; } = new();
        public List<decimal> Fares { get; set; } = new();
        public List<int> Statuses { get; set; } = new();
        public List<DateTime> CreatedAts { get; set; } = new();
        public List<DateTime?> AcceptedAts { get; set; } = new();
        public List<DateTime?> BoardedAts { get; set; } = new();
        public List<DateTime?> CompletedAts { get; set; } = new();
        public List<DateTime?> EndedAts { get; set; } = new();

        public int IndexOf(Guid rideRequestId) =>
            RideRequestIds.IndexOf(rideRequestId);

        public BookingStatus GetStatus(int index)
        {
            if (index < 0 || index >= Statuses.Count)
            {
                throw new IndexOutOfRangeException("Invalid passenger index");
            }

            return (BookingStatus)Statuses[index];
        }

        public int PassengerCount => RideRequestIds.Count;

    }
}
