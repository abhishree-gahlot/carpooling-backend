using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Infrastructure.Repositories {
    public class RideSessionRepository : IRideSessionRepository {
        private readonly AppDbContext _context;

        public RideSessionRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<List<Guid>> GetActiveDriverIdsAsync() {
            return await _context.RideSessions.Where(r => r.IsActive).Select(r => r.DriverId).Distinct().ToListAsync();
        }

        public async Task<RideSession?> GetActiveSessionByDriverIdAsync(Guid driverId) {
            return await _context.RideSessions.Include(r => r.Driver).Include(r => r.Vehicle).Include(r => r.Passenger).FirstOrDefaultAsync(r => r.DriverId == driverId && r.IsActive);
        }

        public async Task<IEnumerable<RideSession>> GetAllAsync() {
            return await _context.RideSessions.Include(r => r.Driver).Include(r => r.Vehicle).Include(r => r.Passenger).ToListAsync();
        }

        public async Task<IEnumerable<RideSession>> GetByDriverIdAsync(Guid driverId) {
            return await _context.RideSessions.Include(r => r.Driver).Include(r => r.Vehicle).Include(r => r.Passenger).Where(r => r.DriverId == driverId).ToListAsync();
        }

        public async Task<RideSession?> GetByIdAsync(Guid rideId) {
            return await _context.RideSessions.Include(r => r.Driver).Include(r => r.Vehicle).Include(r => r.Passenger).FirstOrDefaultAsync(r => r.Id == rideId);
        }

        public async Task AddAsync(RideSession rideSession) {
            await _context.RideSessions.AddAsync(rideSession);
        }

        public void Delete(RideSession rideSession) {
            _context.RideSessions.Remove(rideSession);
        }
        public async Task<bool> SaveChangesAsync() {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(RideSession rideSession) {
            _context.RideSessions.Update(rideSession);
        }
    }
}
