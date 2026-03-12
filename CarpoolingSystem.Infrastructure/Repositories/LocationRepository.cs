using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Infrastructure.Repositories {
    public class LocationRepository : ILocationRepository {

        private readonly AppDbContext _context;

        public LocationRepository(AppDbContext context) {
            _context = context;
            
        }
        public async Task AddAsync(Location location) {
            await _context.Locations.AddAsync(location);
        }

        public async Task<IEnumerable<Location>> GetAllActiveDriverLocationsAsync(List<Guid> activeDriverIds) {
            return await _context.Locations.Include(l=>l.User).Where(l => activeDriverIds.Contains(l.UserId))
                .ToListAsync();
        }

        public async Task<Location?> GetByUserIdAsync(Guid userId) {
            return await _context.Locations.FirstOrDefaultAsync(loc => loc.UserId == userId);
        }

        public async Task<bool> SaveChangesAsync() {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Location location) {
            _context.Locations.Update(location);
        }
    }
}
