using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Helper;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;

namespace CarpoolingSystem.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRideSessionRepository _rideSessionRepository;
        private readonly IRideRequestRepository _rideRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubService _hubService;
        private readonly IDriverHistoryService _driverHistoryService;
        private const decimal RatePerKm = 9.0m;

        public BookingService(
            IBookingRepository bookingRepository,
            IRideSessionRepository rideSessionRepository, 
            IUserRepository userRepository, 
            IHubService hubService,
            IDriverHistoryService driverHistoryService,
            IRideRequestRepository rideRequestRepository) {
            _bookingRepository = bookingRepository;
            _rideSessionRepository = rideSessionRepository;
            _rideRequestRepository = rideRequestRepository;
            _userRepository = userRepository;
            _hubService = hubService;
            _driverHistoryService = driverHistoryService;
        }

        public async Task<Booking> AcceptBookingAsync(Guid rideRequestId, Guid sessionId)
        {
            var session = await _rideSessionRepository.GetByIdAsync(sessionId)
                ?? throw new Exception("Ride session not found.");

            if (!session.IsActive)
                throw new Exception("This ride session is no longer active.");

            if (session.AvailableSeats <= 0)
                throw new Exception("No available seats in this session.");

            var rideRequest = await _rideRequestRepository.GetByIdAsync(rideRequestId)
                ?? throw new Exception("Ride request not found.");

            var passenger = await _userRepository.GetByIdAsync(rideRequest.PassengerId)
                ?? throw new Exception("Passenger not found.");

            if (string.IsNullOrEmpty(passenger.Pin))
                throw new Exception("Passenger has not been assigned a PIN.");

            double distanceKm = DistanceHelper.CalculateDistanceKm(
                rideRequest.PickupLatitude, rideRequest.PickupLongitude,
                rideRequest.DestinationLatitude, rideRequest.DestinationLongitude);

            decimal fare = Math.Round((decimal)distanceKm * RatePerKm, 2);

            var booking = await _bookingRepository.GetBySessionIdAsync(sessionId);

            if (booking == null)
            {
                booking = new Booking
                {
                    BookingId = Guid.NewGuid(),
                    SessionId = sessionId,
                    RideRequestIds = new List<Guid>(),
                    PINs = new List<string>(),
                    Fares = new List<decimal>(),
                    Statuses = new List<int>(),
                    CreatedAts = new List<DateTime>(),
                    AcceptedAts = new List<DateTime?>(),
                    BoardedAts = new List<DateTime?>(),
                    CompletedAts = new List<DateTime?>(),
                    EndedAts = new List<DateTime?>()
                };
                await _bookingRepository.AddAsync(booking);
                await _bookingRepository.SaveChangesAsync();
            }
            else
            {
                if (booking.RideRequestIds.Contains(rideRequestId))
                    throw new Exception("This passenger has already been accepted in this session.");
            }

            booking.RideRequestIds.Add(rideRequestId);
            booking.PINs.Add(passenger.Pin);
            booking.Fares.Add(fare);
            booking.Statuses.Add((int)BookingStatus.Accepted);
            booking.CreatedAts.Add(DateTime.UtcNow);
            booking.AcceptedAts.Add(DateTime.UtcNow);
            booking.BoardedAts.Add(null);
            booking.CompletedAts.Add(null);
            booking.EndedAts.Add(null);

            session.Status = RideSessionStatus.InProgress;
            session.DriverAvailability = DriverAvailability.Busy;
            _rideSessionRepository.Update(session);

            rideRequest.RideRequestStatus = RideRequestStatus.Accepted;
            rideRequest.RespondedAt = DateTime.UtcNow;
            _rideRequestRepository.Update(rideRequest);
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> VerifyPinAsync(Guid bookingId, Guid rideRequestId, BookingVerifyPinDto dto)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId)
                ?? throw new Exception("Booking not found.");

            int index = booking.IndexOf(rideRequestId);
            if (index == -1)
                throw new Exception("Passenger not found in this booking.");

            if (booking.GetStatus(index) != BookingStatus.Accepted)
                throw new Exception("Booking must be accepted before PIN verification.");

            if (booking.PINs[index] != dto.PIN)
                throw new Exception("Invalid PIN. Please check with your driver.");

            booking.Statuses[index] = (int)BookingStatus.Boarded;
            booking.BoardedAts[index] = DateTime.UtcNow;

            var session = await _rideSessionRepository.GetByIdAsync(booking.SessionId)
                ?? throw new Exception("Session not found.");

            session.AvailableSeats--;

            if (session.AvailableSeats > 0)
            {
                session.Status = RideSessionStatus.Waiting;
                session.DriverAvailability = DriverAvailability.Available;
            }
            else
            {
                session.Status = RideSessionStatus.InProgress;
                session.DriverAvailability = DriverAvailability.Busy;
            }

            _rideSessionRepository.Update(session);
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> RejectBookingAsync(Guid bookingId, Guid rideRequestId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId)
                ?? throw new Exception("Booking not found.");

            int index = booking.IndexOf(rideRequestId);
            if (index == -1)
                throw new Exception("Passenger not found in this booking.");

            if (booking.GetStatus(index) != BookingStatus.Pending)
                throw new Exception("Only pending bookings can be rejected.");

            booking.Statuses[index] = (int)BookingStatus.Rejected;
            booking.EndedAts[index] = DateTime.UtcNow;

            var session = await _rideSessionRepository.GetByIdAsync(booking.SessionId);
            if (session != null)
            {
                session.Status = RideSessionStatus.Waiting;
                session.DriverAvailability = DriverAvailability.Available;
                _rideSessionRepository.Update(session);
            }

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            await _hubService.NotifyPassengerAsync(
                booking.RideRequest.PassengerId,
                "PinVerified",
                new { success = true }
            );

            return booking;
        }

        public async Task<Booking> CompleteBookingAsync(Guid bookingId, Guid rideRequestId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId)
                ?? throw new Exception("Booking not found.");

            int index = booking.IndexOf(rideRequestId);
            if (index == -1)
                throw new Exception("Passenger not found in this booking.");

            if (booking.GetStatus(index) != BookingStatus.Boarded)
                throw new Exception("Only boarded bookings can be completed.");

            booking.Statuses[index] = (int)BookingStatus.Completed;
            booking.CompletedAts[index] = DateTime.UtcNow;
            booking.EndedAts[index] = DateTime.UtcNow;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            // Aggressive history generation
            try {
                await _driverHistoryService.CreateFromSessionAsync(booking.SessionId);
            } catch (Exception ex) {
                Console.WriteLine($"[History Worker] Failed to cleanly upsert history: {ex.Message}");
            }

            return booking;
        }

        public async Task CompleteJourneyAsync(Guid sessionId)
        {
            var session = await _rideSessionRepository.GetByIdAsync(sessionId)
                ?? throw new Exception("Session not found.");

            var booking = await _bookingRepository.GetBySessionIdAsync(sessionId);
            if (booking != null)
            {
                for (int i = 0; i < booking.Statuses.Count; i++)
                {
                    if ((BookingStatus)booking.Statuses[i] == BookingStatus.Boarded)
                    {
                        booking.Statuses[i] = (int)BookingStatus.Completed;
                        booking.CompletedAts[i] = DateTime.UtcNow;
                        booking.EndedAts[i] = DateTime.UtcNow;
                    }
                }
                _bookingRepository.Update(booking);
                await _bookingRepository.SaveChangesAsync();
            }

            session.IsActive = false;
            session.Status = RideSessionStatus.Completed;
            session.DriverAvailability = DriverAvailability.Busy;
            session.EndedAt = DateTime.UtcNow;
            _rideSessionRepository.Update(session);
            await _rideSessionRepository.SaveChangesAsync();
        }

        public async Task<Booking> CancelBookingAsync(Guid bookingId, Guid rideRequestId, Guid passengerId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId)
                ?? throw new Exception("Booking not found.");

            int index = booking.IndexOf(rideRequestId);
            if (index == -1)
                throw new Exception("Passenger not found in this booking.");

            var rideRequest = await _rideRequestRepository.GetByIdAsync(rideRequestId)
                ?? throw new Exception("Ride request not found.");

            if (rideRequest.PassengerId != passengerId)
                throw new Exception("You can only cancel your own bookings.");

            if (booking.GetStatus(index) == BookingStatus.Boarded ||
                booking.GetStatus(index) == BookingStatus.Completed)
                throw new Exception("Cannot cancel a booking that is already boarded or completed.");

            booking.Statuses[index] = (int)BookingStatus.Cancelled;
            booking.EndedAts[index] = DateTime.UtcNow;

            var session = await _rideSessionRepository.GetByIdAsync(booking.SessionId);
            if (session != null)
            {
                session.AvailableSeats++;
                session.Status = RideSessionStatus.Waiting;
                session.DriverAvailability = DriverAvailability.Available;
                _rideSessionRepository.Update(session);
            }

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByPassengerIdAsync(Guid passengerId)
        {
            var rideRequests = await _rideRequestRepository.GetByPassengerIdAsync(passengerId);
            var passengerRideRequestIds = new HashSet<Guid>(rideRequests.Select(r => r.Id));
            var allData = await _bookingRepository.GetAllAsync();
            return allData.Where(b => b.RideRequestIds.Any(id => passengerRideRequestIds.Contains(id)));
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDriverIdAsync(Guid driverId)
        {
            return await _bookingRepository.GetByDriverIdAsync(driverId);
        }

        public async Task<Booking?> GetBookingsBySessionIdAsync(Guid sessionId)
        {
            return await _bookingRepository.GetBySessionIdAsync(sessionId);
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid bookingId)
        {
            return await _bookingRepository.GetByIdAsync(bookingId);
        }

        public async Task<bool> DeleteBookingAsync(Guid bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId)
                ?? throw new Exception("Booking not found.");

            _bookingRepository.Delete(booking);
            return await _bookingRepository.SaveChangesAsync();
        }
    }
}