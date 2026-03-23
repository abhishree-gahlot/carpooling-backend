using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Infrastructure.Repositories {
    public class DriverHistoryRepository : IDriverHistoryRepository {
        private readonly AppDbContext _context;
        public DriverHistoryRepository(AppDbContext context) {
            _context=context;
        }

        public async Task<DriverHistory?> GetByIdAsync(Guid driverHistoryId) {
            return await _context.DriverHistories
                .Include(h => h.Driver)
                .Include(h => h.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .Include(h => h.Passengers)
                    .ThenInclude(p => p.RideRequests)
                        .ThenInclude(r => r.Passenger)
                .FirstOrDefaultAsync(h => h.DriverHistoryId == driverHistoryId);
        }

        public async Task<IEnumerable<DriverHistory>> GetAllAsync() {
            return await _context.DriverHistories
                .Include(h => h.Driver)
                .Include(h => h.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .Include(h => h.Passengers)
                    .ThenInclude(p => p.RideRequests)
                        .ThenInclude(r => r.Passenger)
                .ToListAsync();
        }
        public async Task<IEnumerable<DriverHistory>> GetByDriverIdAsync(Guid driverId) {
            return await _context.DriverHistories
                .Include(h => h.Driver)
                .Include(h => h.RideSession)
                    .ThenInclude(s => s.Vehicle)
                .Include(h => h.Passengers)
                    .ThenInclude(p => p.RideRequests)
                        .ThenInclude(r => r.Passenger)
                .Where(h => h.DriverId == driverId)
                .OrderByDescending(h => h.DateAndTime)
                .ToListAsync();
        }



        public async Task<DriverHistory?> GetByRideSessionIdAsync(Guid rideSessionId) {
            return await _context.DriverHistories
                .Include(h => h.Driver)
                .Include(h => h.RideSession)
                .Include(h => h.Passengers)
                .FirstOrDefaultAsync(h => h.RideSessionId == rideSessionId);
        }
        public async Task AddAsync(DriverHistory driverHistory) {
            await _context.DriverHistories.AddAsync(driverHistory);
        }

        public async Task<bool> SaveChangesAsync() {
            return await _context.SaveChangesAsync() > 0;
        }
        public void Delete(DriverHistory driverHistory) {
            _context.DriverHistories.Remove(driverHistory);
        }
        public void Update(DriverHistory driverHistory) {
            _context.DriverHistories.Update(driverHistory);
        }
    }
}
