using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Services {
    public class RideSessionService: IRideSessionService {
        private readonly IRideSessionRepository _rideSessionRepository;
        private readonly CarpoolingSystem.Domain.Repositories.IVehicleRepository _vehicleRepository;

        public RideSessionService(IRideSessionRepository rideSessionRepository, CarpoolingSystem.Domain.Repositories.IVehicleRepository vehicleRepository) {
            _rideSessionRepository = rideSessionRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<RideSession> CreateSessionAsync(RideSessionCreateDto dto, Guid driverId) {
            var existingSessions=await _rideSessionRepository.GetActiveSessionByDriverIdAsync(driverId);

            if (existingSessions != null) {
                existingSessions.IsActive = false;
                existingSessions.EndedAt = DateTime.UtcNow;
                _rideSessionRepository.Update(existingSessions);
                await _rideSessionRepository.SaveChangesAsync();
            }

            var vehicle=await _vehicleRepository.GetByIdAsync(dto.VehicleId);

            if (vehicle == null)
                throw new Exception("Vehicle not found.");

            if (vehicle.DriverId != driverId)
                throw new Exception("This vehicle does not belong to you.");

            if (!vehicle.IsActive)
                throw new Exception("Vehicle is not active.");
            int seats = dto.AvailableSeats ?? vehicle.MaxSeats;

            var session = new RideSession {
                DriverId = driverId,
                VehicleId = dto.VehicleId,
                TotalSeats = vehicle.MaxSeats,
                AvailableSeats = seats,
                Pickup = dto.Pickup,
                Dropoff = dto.Dropoff,
                IsActive = true,
                StartedAt = DateTime.UtcNow
            };

            await _rideSessionRepository.AddAsync(session);
            await _rideSessionRepository.SaveChangesAsync();

            return session;
        }

        public async Task<RideSession> UpdateSessionAsync(
            Guid rideId, RideSessionUpdateDto dto) {
            var session = await _rideSessionRepository.GetByIdAsync(rideId);

            if (session == null)
                throw new Exception("Ride session not found.");

            if (dto.Pickup != null)
                session.Pickup = dto.Pickup;

            if (dto.Dropoff != null)
                session.Dropoff = dto.Dropoff;
            if (dto.AvailableSeats.HasValue)
                session.AvailableSeats = dto.AvailableSeats.Value;

            if (dto.IsActive.HasValue)
                session.IsActive = dto.IsActive.Value;

            _rideSessionRepository.Update(session);
            await _rideSessionRepository.SaveChangesAsync();

            return session;
        }

        public async Task<RideSession> GoOfflineAsync(Guid driverId) {
            var session = await _rideSessionRepository
                .GetActiveSessionByDriverIdAsync(driverId);

            if (session == null)
                throw new Exception("No active session found.");

            session.IsActive = false;
            session.EndedAt = DateTime.UtcNow;

            _rideSessionRepository.Update(session);
            await _rideSessionRepository.SaveChangesAsync();

            return session;
        }

        public async Task<IEnumerable<RideSession>> GetAllSessionsAsync() {
            return await _rideSessionRepository.GetAllAsync();
        }

        public async Task<IEnumerable<RideSession>> GetSessionsByDriverIdAsync(Guid driverId) {
            return await _rideSessionRepository.GetByDriverIdAsync(driverId);
        }

        public async Task<RideSession?> GetSessionByIdAsync(Guid rideId) {
            return await _rideSessionRepository.GetByIdAsync(rideId);
        }

        public async Task<bool> DeleteSessionAsync(Guid rideId) {
            var session = await _rideSessionRepository.GetByIdAsync(rideId);

            if (session == null)
                throw new Exception("Ride session not found.");

            _rideSessionRepository.Delete(session);
            return await _rideSessionRepository.SaveChangesAsync();
        }
    }
}
