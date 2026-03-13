using CarpoolingSystem.Domain.Entities;

namespace CarpoolingSystem.Domain.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task<bool> SaveChangesAsync();
    }
}
