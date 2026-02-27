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

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                                 .Include(vehicleDetails => vehicleDetails.Driver)
                                 .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            return await _context.Vehicles
                                 .Include(vehicleDetails=> vehicleDetails.Driver)
                                 .FirstOrDefaultAsync(vehicleDetails => vehicleDetails.VehicleId == id);
        }

        public async Task<Vehicle?> GetByDriverIdAsync(Guid id)
        {
            var vehicle = await _context.Vehicles
                                .Include(v => v.Driver)
                                .FirstOrDefaultAsync(v => v.VehicleId == id);

            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with Id {id} not found");

            return vehicle;
        }
            
        public void Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
        }

        public async Task<bool> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}