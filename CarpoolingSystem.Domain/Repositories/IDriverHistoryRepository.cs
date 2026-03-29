using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Repositories {
    public interface IDriverHistoryRepository {
        Task<DriverHistory?> GetByIdAsync(Guid driverHistoryId);
        Task<IEnumerable<DriverHistory>> GetAllAsync();
        Task<IEnumerable<DriverHistory>> GetByDriverIdAsync(Guid driverId);
        Task<DriverHistory?> GetByRideSessionIdAsync(Guid rideSessionId);
        Task AddAsync(DriverHistory driverHistory);
        void Update(DriverHistory driverHistory);
        void Delete(DriverHistory driverHistory);
        Task<bool> SaveChangesAsync();

    }
}
