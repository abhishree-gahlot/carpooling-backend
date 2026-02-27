using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Enums;

namespace CarpoolingSystem.Application.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }
        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }
        public async Task<Vehicle> CreateVehicleAsync(VehicleCreateDTO dto, UserRole userRole, Guid currentUserId)
        {
            if (userRole != UserRole.Driver || dto.DriverId != currentUserId)
            {
                throw new UnauthorizedAccessException("Only the assigned driver can create their vehicle.");
            }

            var existingVehicle = await _vehicleRepository.GetByDriverIdAsync(currentUserId);

            if (existingVehicle != null && existingVehicle.IsActive)
            {
                throw new InvalidOperationException("Driver already has a vehicle which is active.");
            }

            if (dto.MaxSeats < 1 || dto.MaxSeats > 6)
            {
                throw new ArgumentException("MaxSeats must be between 1 and 6.");
            }

            var vehicle = new Vehicle
            {
                VehicleId = Guid.NewGuid(),
                VehicleName = dto.VehicleName,
                DriverId = dto.DriverId,
                MaxSeats = dto.MaxSeats,
                LicensePlate = dto.LicensePlate,
                IsActive = true
            };

            await _vehicleRepository.AddAsync(vehicle);
            var saved = await _vehicleRepository.SaveChangesAsync();
            if (!saved)
            {
                throw new Exception("Failed to save vehicle.");
            }

            return vehicle;
        }

        public async Task<Vehicle> UpdateVehicleAsync(Guid id, VehicleUpdateDTO dto)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException("Vehicle not found.");
            }

            vehicle.VehicleName = dto.VehicleName;
            vehicle.MaxSeats = dto.MaxSeats;
            vehicle.IsActive = dto.IsActive;
            vehicle.LicensePlate = dto.LicensePlate;

            _vehicleRepository.Update(vehicle);
            var saved = await _vehicleRepository.SaveChangesAsync();
            if (!saved)
            {
                throw new Exception("Failed to update vehicle.");
            }

            return vehicle;
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException("Vehicle not found.");
            }

            _vehicleRepository.Delete(vehicle);
            var saved = await _vehicleRepository.SaveChangesAsync();
            if (!saved)
            {
                throw new Exception("Failed to delete vehicle.");
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }
    }
}