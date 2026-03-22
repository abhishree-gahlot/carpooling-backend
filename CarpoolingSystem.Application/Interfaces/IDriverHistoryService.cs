using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces {
    public interface IDriverHistoryService {
        Task<DriverHistory> CreateFromSessionAsync(Guid rideSessionId);

        Task<DriverHistory?> GetByIdAsync(Guid driverHistoryId);
        Task<IEnumerable<DriverHistory>> GetAllAsync();
        Task<IEnumerable<DriverHistory>> GetByDriverIdAsync(Guid driverId);
        Task<bool> DeleteAsync(Guid driverHistoryId);
    }
}
