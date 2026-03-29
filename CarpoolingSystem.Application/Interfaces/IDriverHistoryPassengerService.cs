using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces {
    public interface IDriverHistoryPassengerService {
            Task<IEnumerable<DriverHistoryPassenger>> GetByPassengerIdAsync(Guid passengerId);
            Task<DriverHistoryPassenger?> GetByIdAsync(Guid passengerHistoryId);
            Task<DriverHistoryPassenger> RateDriverAsync(Guid passengerHistoryId, RateDriverDto dto);
    }
}
