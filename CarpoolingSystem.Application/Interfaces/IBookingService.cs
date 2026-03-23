using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces {
    public interface IBookingService {

        Task<Booking> CreateBookingAsync(BookingCreateDto dto);
        Task<Booking> AcceptBookingAsync(Guid rideRequestId, Guid sessionId);
        Task<Booking> RejectBookingAsync(Guid bookingId);
        Task<Booking> VerifyPinAsync(Guid bookingId, BookingVerifyPinDto dto);
        Task<Booking> CompleteBookingAsync(Guid bookingId);
        Task<Booking> CancelBookingAsync(Guid bookingId, Guid passengerId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByPassengerIdAsync(Guid passengerId);
        Task<IEnumerable<Booking>> GetBookingsByDriverIdAsync(Guid driverId);
        Task<IEnumerable<Booking>> GetBookingsBySessionIdAsync(Guid sessionId);
        Task<Booking?> GetBookingByIdAsync(Guid bookingId);
        Task<bool> DeleteBookingAsync(Guid bookingId);
    }
}
