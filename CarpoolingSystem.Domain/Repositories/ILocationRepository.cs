using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Repositories {
    public interface ILocationRepository {
        Task<Location> GetByUserIdAsync(Guid userId);

        Task<IEnumerable<Location>> GetAllActiveDriverLocationsAsync(List<Guid> activeDriverIds);

        Task AddAsync(Location location);

        void Update(Location location);
        Task<bool> SaveChangesAsync();

    }
}
