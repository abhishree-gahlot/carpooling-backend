using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Helper;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Services {
    public class BookingService : IBookingService {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRideSessionRepository _rideSessionRepository;
        private readonly IUserRepository _userRepository;
        private const decimal RatePerKm = 9.0m;

        public BookingService(IBookingRepository bookingRepository,IRideSessionRepository rideSessionRepository, IUserRepository userRepository) {
            _bookingRepository = bookingRepository;
            _rideSessionRepository = rideSessionRepository;
            _userRepository=userRepository;
        }

        public async Task<Booking> CreateBookingAsync(BookingCreateDto dto) {
            var session = await _rideSessionRepository.GetByIdAsync(dto.SessionId);

            if (session == null)
                throw new Exception("Ride session not found.");

            if (!session.IsActive)
                throw new Exception("This ride session is no longer active.");

            if (session.AvailableSeats <= 0)
                throw new Exception("No available seats in this session.");

            var existing = await _bookingRepository
                .GetByRideRequestIdAsync(dto.RideRequestId);

            if (existing != null)
                throw new Exception("A booking already exists for this ride request.");

            var booking = new Booking {
                BookingId = Guid.NewGuid(),
                RideRequestId = dto.RideRequestId,
                SessionId = dto.SessionId,
                PIN = string.Empty,
                Fare=0,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            return await _bookingRepository.GetByIdAsync(booking.BookingId)?? throw new Exception("Failed to retrieve created booking.");
        }

        public async Task<Booking> AcceptBookingAsync(Guid bookingId) {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new Exception("Booking not found.");

            if (booking.Status != BookingStatus.Pending)
                throw new Exception("Only pending bookings can be accepted.");

            var passenger = await _userRepository
                .GetByIdAsync(booking.RideRequest.PassengerId);

            if (passenger == null)
                throw new Exception("Passenger not found.");

            if (string.IsNullOrEmpty(passenger.Pin))
                throw new Exception(
                    "Passenger has not been assigned a PIN");

            double distanceKm = DistanceHelper.CalculateDistanceKm(
                booking.RideRequest.PickupLatitude,
                booking.RideRequest.PickupLongitude,
                booking.RideRequest.DestinationLatitude,
                booking.RideRequest.DestinationLongitude);

            decimal fare = Math.Round(
                (decimal)distanceKm * RatePerKm, 2);

            booking.PIN = passenger.Pin;
            booking.Fare = fare;
            booking.Status = BookingStatus.Accepted;
            booking.AcceptedAt = DateTime.UtcNow;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> RejectBookingAsync(Guid bookingId) {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new Exception("Booking not found.");

            if (booking.Status != BookingStatus.Pending)
                throw new Exception("Only pending bookings can be rejected.");

            booking.Status = BookingStatus.Rejected;
            booking.EndedAt = DateTime.UtcNow;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> VerifyPinAsync(
            Guid bookingId, BookingVerifyPinDto dto) {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new Exception("Booking not found.");

            if (booking.Status != BookingStatus.Accepted)
                throw new Exception("Booking must be accepted before PIN verification.");

            if (booking.PIN != dto.PIN)
                throw new Exception("Invalid PIN. Please check with your driver.");

            booking.Status = BookingStatus.Boarded;
            booking.BoardedAt = DateTime.UtcNow;

            var session = await _rideSessionRepository
                .GetByIdAsync(booking.SessionId);

            if (session != null && session.AvailableSeats > 0) {
                session.AvailableSeats--;
                _rideSessionRepository.Update(session);
            }

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> CompleteBookingAsync(Guid bookingId) {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new Exception("Booking not found.");

            if (booking.Status != BookingStatus.Boarded)
                throw new Exception("Only boarded bookings can be completed.");

            booking.Status = BookingStatus.Completed;
            booking.CompletedAt = DateTime.UtcNow;
            booking.EndedAt = DateTime.UtcNow;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> CancelBookingAsync(
            Guid bookingId, Guid passengerId) {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new Exception("Booking not found.");

            if (booking.RideRequest.PassengerId != passengerId)
                throw new Exception("You can only cancel your own bookings.");

            if (booking.Status == BookingStatus.Boarded ||
                booking.Status == BookingStatus.Completed)
                throw new Exception("Cannot cancel a booking that is already boarded or completed.");

            booking.Status = BookingStatus.Cancelled;
            booking.EndedAt = DateTime.UtcNow;

            _bookingRepository.Update(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync() {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByPassengerIdAsync(
            Guid passengerId) {
            return await _bookingRepository.GetByPassengerIdAsync(passengerId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDriverIdAsync(
            Guid driverId) {
            return await _bookingRepository.GetByDriverIdAsync(driverId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsBySessionIdAsync(
            Guid sessionId) {
            return await _bookingRepository.GetBySessionIdAsync(sessionId);
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid bookingId) {
            return await _bookingRepository.GetByIdAsync(bookingId);
        }

        public async Task<bool> DeleteBookingAsync(Guid bookingId) {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new Exception("Booking not found.");

            _bookingRepository.Delete(booking);
            return await _bookingRepository.SaveChangesAsync();
        }
    }
}
