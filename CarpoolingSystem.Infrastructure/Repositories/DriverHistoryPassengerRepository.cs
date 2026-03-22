using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Infrastructure.Repositories {
    public class DriverHistoryPassengerRepository : IDriverHistoryPassengerRepository {
        private readonly AppDbContext _context;

        public DriverHistoryPassengerRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<DriverHistoryPassenger?> GetByIdAsync(Guid passengerHistoryId) {
            return await _context.DriverHistoryPassengers
                .Include(p => p.DriverHistory)
                    .ThenInclude(h => h.Driver)
                .Include(p => p.RideRequests)
                    .ThenInclude(r => r.Passenger)
                .FirstOrDefaultAsync(p => p.PassengerHistoryId == passengerHistoryId);
        }

        public async Task<IEnumerable<DriverHistoryPassenger>> GetByDriverHistoryIdAsync(Guid driverHistoryId) {
            return await _context.DriverHistoryPassengers
                .Include(p => p.DriverHistory)
                    .ThenInclude(h => h.Driver)
                .Include(p => p.RideRequests)
                    .ThenInclude(r => r.Passenger)
                .Where(p => p.DriverHistoryId == driverHistoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DriverHistoryPassenger>> GetByPassengerIdAsync(Guid passengerId) {
            return await _context.DriverHistoryPassengers
                .Include(p => p.DriverHistory)
                    .ThenInclude(h => h.Driver)
                .Include(p => p.DriverHistory)
                    .ThenInclude(h => h.RideSession)
                        .ThenInclude(s => s.Vehicle)
                .Include(p => p.RideRequests)
                    .ThenInclude(r => r.Passenger)
                .Where(p => p.RideRequests.PassengerId == passengerId)
                .OrderByDescending(p => p.PickupTime)
                .ToListAsync();
        }

        public async Task AddAsync(DriverHistoryPassenger passengerHistory) {
            await _context.DriverHistoryPassengers.AddAsync(passengerHistory);
        }

        public void Update(DriverHistoryPassenger passengerHistory) {
            _context.DriverHistoryPassengers.Update(passengerHistory);
        }

        public void Delete(DriverHistoryPassenger passengerHistory) {
            _context.DriverHistoryPassengers.Remove(passengerHistory);
        }

        public async Task<bool> SaveChangesAsync() {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
