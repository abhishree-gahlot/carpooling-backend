using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
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

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}