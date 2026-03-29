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

        private IQueryable<Booking> WithIncludes()
        {
            return _context.Bookings
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Driver)
                .Include(b => b.RideSession)
                    .ThenInclude(s => s.Vehicle);
        }

        public async Task<Booking?> GetByIdAsync(Guid bookingId) {
            return await WithIncludes()
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<Booking?> GetBySessionIdAsync(Guid sessionId)
        {
            return await WithIncludes()
                .FirstOrDefaultAsync(b => b.SessionId == sessionId);
        }

        public async Task<Booking?> GetByRideRequestIdAsync(Guid rideRequestId)
        {
            var allData = await WithIncludes().ToListAsync();
            return allData.FirstOrDefault(b => b.RideRequestIds.Contains(rideRequestId));
        }

        public async Task<IEnumerable<Booking>> GetAllAsync() {
            return await WithIncludes().ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByPassengerIdAsync(Guid passengerId) {
            var allData = await WithIncludes().ToListAsync();
            return allData.Where(b => b.RideRequestIds.Contains(passengerId));
        }

        public async Task<IEnumerable<Booking>> GetByDriverIdAsync(Guid driverId) {
            return await WithIncludes()
                .Where(b => b.RideSession.DriverId == driverId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllBySessionIdAsync(Guid sessionId)
        {
            return await WithIncludes()
                   .Where(b => b.SessionId == sessionId)
                   .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status) {
            var allData = await WithIncludes().ToListAsync();
            return allData.Where(b => b.Statuses.Contains((int)status));
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
