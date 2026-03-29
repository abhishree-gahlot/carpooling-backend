using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Domain.Repositories {
    public interface IDriverHistoryPassengerRepository {
        Task<DriverHistoryPassenger?> GetByIdAsync(Guid passengerHistoryId);
        Task<IEnumerable<DriverHistoryPassenger>> GetByDriverHistoryIdAsync(Guid driverHistoryId);
        Task<IEnumerable<DriverHistoryPassenger>> GetByPassengerIdAsync(Guid passengerId);

        Task AddAsync(DriverHistoryPassenger passengerHistory);
        void Update(DriverHistoryPassenger passengerHistory);
        void Delete(DriverHistoryPassenger passengerHistory);
        Task<bool> SaveChangesAsync();
    }
}
