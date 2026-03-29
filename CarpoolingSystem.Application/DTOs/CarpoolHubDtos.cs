namespace CarpoolingSystem.Application.DTOs
{
    // [CARPOOL NEW] Passenger sends mid-trip request to driver
    public class NotifyDriverNewPassengerRequestDto
    {
        public Guid DriverId { get; set; }
        public Guid RequestId { get; set; }
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string PickupName { get; set; } = string.Empty;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string DestinationName { get; set; } = string.Empty;
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
    }

    // [CARPOOL NEW] Driver broadcasts seat count change to session group
    public class NotifySeatCountDto
    {
        public Guid SessionId { get; set; }
        public int AvailableSeats { get; set; }
        public int TotalSeats { get; set; }
    }

    // [CARPOOL NEW] Driver accepts mid-trip passenger — notifies that passenger
    public class NotifyPassengerRequestAcceptedDto
    {
        public Guid PassengerId { get; set; }
        public Guid RequestId { get; set; }
        public Guid BookingId { get; set; }
    }
}