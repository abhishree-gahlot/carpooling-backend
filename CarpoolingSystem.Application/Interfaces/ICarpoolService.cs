using CarpoolingSystem.Domain.Entities;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface ICarpoolService
    {
        Task<Booking> AcceptRequestAsync(Guid sessionId, Guid requestId);
        Task<bool> VerifyPinAsync(Guid bookingId, string enteredPin);
        Task CompleteJourneyAsync(Guid sessionId);
        Task<SessionStatusDto> GetSessionStatusAsync(Guid sessionId);
        Task<List<PendingRequestDto>> GetPendingRequestsForSessionAsync(Guid sessionId);
    }

    // Response DTOs
    public class SessionStatusDto
    {
        public Guid SessionId { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int OccupiedSeats { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DriverStatus { get; set; } = string.Empty;
        public List<OnboardedPassengerDto> OnboardedPassengers { get; set; } = new();
    }

    public class OnboardedPassengerDto
    {
        public Guid BookingId { get; set; }
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string PickupName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? BoardedAt { get; set; }
    }

    public class PendingRequestDto
    {
        public Guid RequestId { get; set; }
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string PickupName { get; set; } = string.Empty;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}