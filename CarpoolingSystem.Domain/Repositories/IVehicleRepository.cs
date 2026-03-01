using CarpoolingSystem.Domain.Entities;

namespace CarpoolingSystem.Domain.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByDriverIdAsync(Guid driverId);
        Task AddAsync(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);
        Task<bool> SaveChangesAsync();
    }
}