using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace CarpoolingSystem.Domain.Repositories {
    public interface IRideSessionRepository {
        Task<RideSession?> GetByIdAsync(Guid rideId);
        Task<IEnumerable<RideSession>> GetAllAsync();
        Task<IEnumerable<RideSession>> GetByDriverIdAsync(Guid driverId);
        Task<RideSession?> GetActiveSessionByDriverIdAsync(Guid driverId);
        Task<List<Guid>> GetActiveDriverIdsAsync();

        Task AddAsync(RideSession rideSession);
        void Update(RideSession rideSession);
        void Delete(RideSession rideSession);
        Task<bool> SaveChangesAsync();
    }
}