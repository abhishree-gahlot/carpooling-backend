namespace CarpoolingSystem.Application.DTOs
{
    public class NearbyDriverDto
    {
        public Guid DriverId { get; set; }
        public Guid SessionId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
        public string DriverAvailability { get; set; } = "Available";  
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceKm { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}