using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Repositories {
    public interface IBookingRepository {

        Task<Booking?> GetByIdAsync(Guid bookingId);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetByPassengerIdAsync(Guid passengerId);
        Task<IEnumerable<Booking>> GetByDriverIdAsync(Guid driverId);
        Task<IEnumerable<Booking>> GetBySessionIdAsync(Guid sessionId);
        Task<Booking?> GetByRideRequestIdAsync(Guid rideRequestId);
        Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status);

        Task AddAsync(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);
        Task<bool> SaveChangesAsync();
    }
}
