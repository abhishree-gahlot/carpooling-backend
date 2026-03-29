using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Services {
    public class DriverHistoryPassengerService : IDriverHistoryPassengerService{

        private readonly IDriverHistoryPassengerRepository _passengerHistoryRepository;
        private readonly IHubService _hubService;

        public DriverHistoryPassengerService(
            IDriverHistoryPassengerRepository passengerHistoryRepository,
            IHubService hubService) {
            _passengerHistoryRepository = passengerHistoryRepository;
            _hubService = hubService;
        }

        public async Task<IEnumerable<DriverHistoryPassenger>> GetByPassengerIdAsync(Guid passengerId) {
            return await _passengerHistoryRepository.GetByPassengerIdAsync(passengerId);
        }

        public async Task<DriverHistoryPassenger?> GetByIdAsync(Guid passengerHistoryId) {
            return await _passengerHistoryRepository.GetByIdAsync(passengerHistoryId);
        }

        public async Task<DriverHistoryPassenger> RateDriverAsync(
            Guid passengerHistoryId, RateDriverDto dto) {
            var entry = await _passengerHistoryRepository.GetByIdAsync(passengerHistoryId);

            if (entry == null)
                throw new Exception("Passenger history entry not found.");

            if (entry.Ratings.HasValue)
                throw new Exception("The driver has already been rated for this ride.");

            entry.Ratings = dto.Ratings;

            _passengerHistoryRepository.Update(entry);
            await _passengerHistoryRepository.SaveChangesAsync();

            if (entry.DriverHistory != null)
            {
                await _hubService.NotifyDriverAsync(entry.DriverHistory.DriverId, "DriverRated", null);
            }

            return entry;
        }
    }
}
