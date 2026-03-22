using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Infrastructure.Repositories {
    public class BookingRepository : IBookingRepository{
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(Guid bookingId) {
            return await _context.Bookings
                .Include(b => b.RideRequest)
                    .ThenInclude(r => r.Passenger)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Driver)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync() {
            return await _context.Bookings
                .Include(b => b.RideRequest)
                    .ThenInclude(r => r.Passenger)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Driver)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByPassengerIdAsync(Guid passengerId) {
            return await _context.Bookings
                .Include(b => b.RideRequest)
                    .ThenInclude(r => r.Passenger)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Driver)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .Where(b => b.RideRequest.PassengerId == passengerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByDriverIdAsync(Guid driverId) {
            return await _context.Bookings
                .Include(b => b.RideRequest)
                    .ThenInclude(r => r.Passenger)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Driver)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .Where(b => b.RideSession.DriverId == driverId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBySessionIdAsync(Guid sessionId) {
            return await _context.Bookings
                .Include(b => b.RideRequest)
                    .ThenInclude(r => r.Passenger)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Driver)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .Where(b => b.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<Booking?> GetByRideRequestIdAsync(Guid rideRequestId) {
            return await _context.Bookings
                .Include(b => b.RideRequest)
                    .ThenInclude(r => r.Passenger)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Driver)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .FirstOrDefaultAsync(b => b.RideRequestId == rideRequestId);
        }

        public async Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status) {
            return await _context.Bookings
                .Include(b => b.RideRequest)
                    .ThenInclude(r => r.Passenger)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Driver)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .Where(b => b.Status == status)
                .ToListAsync();
        }

        public async Task AddAsync(Booking booking) {
            await _context.Bookings.AddAsync(booking);
        }

        public void Update(Booking booking) {
            _context.Bookings.Update(booking);
        }

        public void Delete(Booking booking) {
            _context.Bookings.Remove(booking);
        }

        public async Task<bool> SaveChangesAsync() {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
