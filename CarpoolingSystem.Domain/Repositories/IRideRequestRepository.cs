using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Repositories {
    public interface IRideRequestRepository {

        Task<RideRequests?> GetByIdAsync(Guid requestId);
        Task<IEnumerable<RideRequests>> GetallAsync();
        Task<IEnumerable<RideRequests>> GetByPassengerIdAsync(Guid passengerId);
        Task<IEnumerable<RideRequests>> GetPendingRequestsAsync(); 
        Task AddAsync(RideRequests rideRequest);
        void Update(RideRequests rideRequest);
        void Delete(RideRequests rideRequest);
        Task<bool> SaveChangesAsync();
    }
}
