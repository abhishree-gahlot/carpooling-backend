using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;

namespace CarpoolingSystem.Application.Services
{
    public class CarpoolService : ICarpoolService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRideSessionRepository _rideSessionRepository;
        private readonly IUserRepository _userRepository;

        public CarpoolService(
            IBookingRepository bookingRepository,
            IRideSessionRepository rideSessionRepository,
            IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _rideSessionRepository = rideSessionRepository;
            _userRepository = userRepository;
        }

        public async Task<Booking> AcceptRequestAsync(Guid sessionId, Guid requestId)
        {
            var session = await _rideSessionRepository.GetByIdAsync(sessionId)
                ?? throw new Exception("Session not found.");

            if (!session.IsActive)
                throw new Exception("Session is no longer active.");

            if (session.AvailableSeats <= 0)
                throw new Exception("No available seats.");

            var existing = await _bookingRepository.GetByRideRequestIdAsync(requestId);

            Booking booking;
            if (existing == null)
            {
                booking = new Booking
                {
                    BookingId = Guid.NewGuid(),
                    RideRequestId = requestId,
                    SessionId = sessionId,
                    PIN = string.Empty,
                    Fare = 0,
                    Status = BookingStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };
                await _bookingRepository.AddAsync(booking);
                await _bookingRepository.SaveChangesAsync();
                booking = await _bookingRepository.GetByIdAsync(booking.BookingId)
                    ?? throw new Exception("Failed to retrieve booking.");
            }
            else
            {
                booking = existing;
            }

            if (booking.Status != BookingStatus.Pending)
                throw new Exception("Only pending bookings can be accepted.");

            var passenger = await _userRepository.GetByIdAsync(booking.RideRequest.PassengerId)
                ?? throw new Exception("Passenger not found.");

            if (string.IsNullOrEmpty(passenger.Pin))
                throw new Exception("Passenger has no PIN assigned.");

            booking.PIN = passenger.Pin;
            booking.Status = BookingStatus.Accepted;
            booking.AcceptedAt = DateTime.UtcNow;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> VerifyPinAsync(Guid bookingId, string enteredPin)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId)
                ?? throw new Exception("Booking not found.");

            if (booking.Status != BookingStatus.Accepted)
                throw new Exception("Booking must be accepted before PIN verification.");

            if (booking.PIN != enteredPin)
                return false;

            booking.Status = BookingStatus.Boarded;
            booking.BoardedAt = DateTime.UtcNow;

            var session = await _rideSessionRepository.GetByIdAsync(booking.SessionId)
                ?? throw new Exception("Session not found.");

            if (session.AvailableSeats > 0)
            {
                session.AvailableSeats--;

                if (session.Status == RideSessionStatus.Waiting)
                    session.Status = RideSessionStatus.InProgress;

                _rideSessionRepository.Update(session);
            }

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return true;
        }

        public async Task CompleteJourneyAsync(Guid sessionId)
        {
            var session = await _rideSessionRepository.GetByIdAsync(sessionId)
                ?? throw new Exception("Session not found.");

            var bookings = await _bookingRepository.GetBySessionIdAsync(sessionId);

            foreach (var booking in bookings.Where(b => b.Status == BookingStatus.Boarded))
            {
                booking.Status = BookingStatus.Completed;
                booking.CompletedAt = DateTime.UtcNow;
                booking.EndedAt = DateTime.UtcNow;
                _bookingRepository.Update(booking);
            }

            session.IsActive = false;
            session.Status = RideSessionStatus.Completed;
            session.EndedAt = DateTime.UtcNow;
            _rideSessionRepository.Update(session);

            await _bookingRepository.SaveChangesAsync();
            await _rideSessionRepository.SaveChangesAsync();
        }

        public async Task<SessionStatusDto> GetSessionStatusAsync(Guid sessionId)
        {
            var session = await _rideSessionRepository.GetByIdAsync(sessionId)
                ?? throw new Exception("Session not found.");

            var bookings = await _bookingRepository.GetBySessionIdAsync(sessionId);
            var boarded = bookings.Where(b => b.Status == BookingStatus.Boarded).ToList();

            var onboarded = boarded.Select(b => new OnboardedPassengerDto
            {
                BookingId = b.BookingId,
                PassengerId = b.RideRequest.PassengerId,
                PassengerName = b.RideRequest.Passenger?.UserName ?? "Passenger",
                PickupName = b.RideRequest.PickupName,
                Status = b.Status.ToString(),
                BoardedAt = b.BoardedAt
            }).ToList();

            return new SessionStatusDto
            {
                SessionId = session.Id,
                TotalSeats = session.TotalSeats,
                AvailableSeats = session.AvailableSeats,
                OccupiedSeats = session.TotalSeats - session.AvailableSeats,
                Status = session.Status.ToString(),
                DriverStatus = session.AvailableSeats > 0 ? "Available" : "Full",
                OnboardedPassengers = onboarded
            };
        }

        public async Task<List<PendingRequestDto>> GetPendingRequestsForSessionAsync(Guid sessionId)
        {
            var bookings = await _bookingRepository.GetBySessionIdAsync(sessionId);

            return bookings
                .Where(b => b.Status == BookingStatus.Pending)
                .Select(b => new PendingRequestDto
                {
                    RequestId = b.RideRequestId,
                    PassengerId = b.RideRequest.PassengerId,
                    PassengerName = b.RideRequest.Passenger?.UserName ?? "Passenger",
                    PickupName = b.RideRequest.PickupName,
                    PickupLatitude = b.RideRequest.PickupLatitude,
                    PickupLongitude = b.RideRequest.PickupLongitude,
                    RequestedAt = b.RideRequest.RequestedAt
                }).ToList();
        }
    }
}