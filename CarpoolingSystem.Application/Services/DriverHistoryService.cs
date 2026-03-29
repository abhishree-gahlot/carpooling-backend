using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Services {
    public class DriverHistoryService : IDriverHistoryService {
        private readonly IDriverHistoryRepository _driverHistoryRepository;
        private readonly IDriverHistoryPassengerRepository _passengerHistoryRepository;
        private readonly IRideSessionRepository _rideSessionRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRideRequestRepository _rideRequestRepository;

        public DriverHistoryService(
            IDriverHistoryRepository driverHistoryRepository,IDriverHistoryPassengerRepository passengerHistoryRepository,
            IRideSessionRepository rideSessionRepository,IBookingRepository bookingRepository, IRideRequestRepository rideRequestRepository) {
            _driverHistoryRepository = driverHistoryRepository;
            _passengerHistoryRepository = passengerHistoryRepository;
            _rideSessionRepository = rideSessionRepository;
            _bookingRepository = bookingRepository;
            _rideRequestRepository = rideRequestRepository;
        }

        public async Task<DriverHistory> CreateFromSessionAsync(Guid rideSessionId) {
            var session = await _rideSessionRepository.GetByIdAsync(rideSessionId);
            if (session == null)
                throw new Exception("Ride session not found.");

            var existing = await _driverHistoryRepository.GetByRideSessionIdAsync(rideSessionId);
            if (existing != null) {
                _driverHistoryRepository.Delete(existing);
                await _driverHistoryRepository.SaveChangesAsync();
            }

            var booking = await _bookingRepository.GetBySessionIdAsync(rideSessionId);

            decimal totalFare = 0;
            var completedPassengerIndexes = new List<int>();

            if (booking != null)
            {
                for (int i = 0; i < booking.PassengerCount; i++)
                {
                    if (booking.GetStatus(i) == BookingStatus.Completed)
                    {
                        completedPassengerIndexes.Add(i);
                        totalFare += booking.Fares[i];
                    }
                }
            }

            var driverHistory = new DriverHistory {
                DriverHistoryId = Guid.NewGuid(),
                RideSessionId = rideSessionId,
                DriverId = session.DriverId,
                StartingLocation = session.PickupName,
                DestinationLocation = session.DestinationName,
                DateAndTime = session.StartedAt,
                DropOffTime = session.EndedAt ?? DateTime.UtcNow,
                TotalFare = totalFare
            };

            await _driverHistoryRepository.AddAsync(driverHistory);
            await _driverHistoryRepository.SaveChangesAsync();

            if (booking != null) {
                foreach (int i in completedPassengerIndexes)
                {
                    Guid rideRequestId = booking.RideRequestIds[i];
                    var rideRequest = await _rideRequestRepository.GetByIdAsync(rideRequestId);

                    var passengerEntry = new DriverHistoryPassenger
                    {
                        PassengerHistoryId = Guid.NewGuid(),
                        DriverHistoryId = driverHistory.DriverHistoryId,
                        RideId = rideRequestId,
                        PassengerName = rideRequest?.Passenger?.UserName ?? string.Empty,
                        Pickup = rideRequest?.PickupName ?? string.Empty,
                        Fare = booking.Fares[i],
                        PickupTime = booking.BoardedAts[i],
                        Ratings = null
                    };

                    await _passengerHistoryRepository.AddAsync(passengerEntry);
                }
            }

            await _passengerHistoryRepository.SaveChangesAsync();
            return await _driverHistoryRepository.GetByIdAsync(driverHistory.DriverHistoryId)
                ?? throw new Exception("Failed to retrieve created driver history.");
        }

        public async Task<DriverHistory?> GetByIdAsync(Guid driverHistoryId) {
            return await _driverHistoryRepository.GetByIdAsync(driverHistoryId);
        }

        public async Task<IEnumerable<DriverHistory>> GetAllAsync() {
            return await _driverHistoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<DriverHistory>> GetByDriverIdAsync(Guid driverId) {
            return await _driverHistoryRepository.GetByDriverIdAsync(driverId);
        }

        public async Task<bool> DeleteAsync(Guid driverHistoryId) {
            var history = await _driverHistoryRepository.GetByIdAsync(driverHistoryId);

            if (history == null)
                throw new Exception("Driver history not found.");

            _driverHistoryRepository.Delete(history);
            return await _driverHistoryRepository.SaveChangesAsync();
        }
    }
}
