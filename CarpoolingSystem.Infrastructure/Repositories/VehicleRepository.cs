//using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarpoolingSystem.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
        }

        public async Task<Vehicle?> GetByIdAsync(Guid vehicleId)
        {
            return await _context.Vehicles
                .Include(vehicle => vehicle.Driver)
                .FirstOrDefaultAsync(vehicle => vehicle.VehicleId == vehicleId);
        }

        public async Task UpdateAsync(Vehicle vehicle) {
            _context.Vehicles.Update(vehicle);
            //await Task.CompletedTask;
        }

        public void Delete(Vehicle vehicle) {
            _context.Vehicles.Remove(vehicle);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync() {
            return await _context.Vehicles
                .Include(v => v.Driver)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByDriverIdAsync(Guid driverId) {
            return await _context.Vehicles
                .Include(v => v.Driver)
                .FirstOrDefaultAsync(v => v.DriverId == driverId && v.IsActive);
        }
    }
}