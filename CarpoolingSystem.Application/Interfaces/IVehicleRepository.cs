using CarpoolingSystem.Domain.Entities;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle vehicle);
        Task<Vehicle?> GetByIdAsync(Guid vehicleId);
        Task UpdateAsync(Vehicle vehicle);
        Task<bool> SaveChangesAsync();
    }
}