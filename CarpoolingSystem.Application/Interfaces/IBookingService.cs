using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces {
    public interface IBookingService {
        Task<Booking> AcceptBookingAsync(Guid rideRequestId, Guid sessionId);
        Task<Booking> RejectBookingAsync(Guid bookingId, Guid rideRequestId);
        Task<Booking> VerifyPinAsync(Guid bookingId, Guid rideRequestId, BookingVerifyPinDto dto);
        Task<Booking> CompleteBookingAsync(Guid bookingId, Guid rideRequestId);
        Task<Booking> CancelBookingAsync(Guid bookingId, Guid rideRequestId, Guid passengerId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByPassengerIdAsync(Guid passengerId);
        Task<IEnumerable<Booking>> GetBookingsByDriverIdAsync(Guid driverId);
        Task<Booking> GetBookingsBySessionIdAsync(Guid sessionId);
        Task<Booking?> GetBookingByIdAsync(Guid bookingId);
        Task<bool> DeleteBookingAsync(Guid bookingId);
    }
}
