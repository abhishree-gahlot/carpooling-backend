using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Infrastructure.Repositories {
    public class RideRequestRepository : IRideRequestRepository {
        private readonly AppDbContext _context;

        public RideRequestRepository(AppDbContext context) {
            _context = context;
        }
        public async Task AddAsync(RideRequests rideRequest) {
            await _context.RideRequests.AddAsync(rideRequest);
        }

        public void Delete(RideRequests rideRequest) {
            _context.RideRequests.Remove(rideRequest);
        }

        public async Task<IEnumerable<RideRequests>> GetallAsync() {
            return await _context.RideRequests.Include(r => r.Passenger).ToListAsync();
        }

        public async Task<RideRequests?> GetByIdAsync(Guid requestId) {
            return await _context.RideRequests.Include(r => r.Passenger).FirstOrDefaultAsync(r => r.Id == requestId);
        }

        public async Task<IEnumerable<RideRequests>> GetByPassengerIdAsync(Guid passengerId) {
            return await _context.RideRequests.Include(r => r.Passenger).Where(r => r.PassengerId == passengerId).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync() {
            return await _context.SaveChangesAsync()>0;
        }

        public void Update(RideRequests rideRequest) {
            _context.RideRequests.Update(rideRequest);
        }

        public async Task<IEnumerable<RideRequests>> GetPendingRequestsAsync() {
            return await _context.RideRequests
                .Include(r => r.Passenger)
                .Where(r => r.RideRequestStatus == RideRequestStatus.Pending)
                .ToListAsync();
        }
    }
}
