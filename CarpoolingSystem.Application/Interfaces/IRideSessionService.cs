using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces {
    public interface IRideSessionService {
        Task<RideSession> CreateSessionAsync(RideSessionCreateDto dto, Guid driverId);
        Task<RideSession> UpdateSessionAsync(Guid rideId, RideSessionUpdateDto dto);
        Task<RideSession> GoOfflineAsync(Guid driverId);
        Task<IEnumerable<RideSession>> GetAllSessionsAsync();
        Task<IEnumerable<RideSession>> GetSessionsByDriverIdAsync(Guid driverId);
        Task<RideSession?> GetSessionByIdAsync(Guid rideId);
        Task<bool> DeleteSessionAsync(Guid rideId);
    }
}
