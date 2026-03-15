using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;

namespace CarpoolingSystem.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }
        public async Task<Vehicle> CreateVehicleAsync(VehicleCreateDTO dto,
            UserRole role,
            Guid userId)
        {
            if (role != UserRole.Driver)
            {
                throw new Exception("Only drivers can register vehicles.");
            }
            var vehicle = new Vehicle
            {
                VehicleName = dto.VehicleName,
                MaxSeats = dto.MaxSeats,
                LicensePlate = dto.LicensePlate,
                DriverId = userId,
                IsActive = true
            };

            await _vehicleRepository.AddAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            return vehicle;
        }
        public async Task<Vehicle> UpdateVehicleAsync(Guid vehicleId, VehicleUpdateDTO dto)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);

            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }
            if (dto.VehicleName != null)
            {
                vehicle.VehicleName = dto.VehicleName;
            }
            if (dto.MaxSeats.HasValue)
            {
                vehicle.MaxSeats = dto.MaxSeats.Value;
            }
            if (dto.LicensePlate != null)
            {
                vehicle.LicensePlate = dto.LicensePlate;
            }
            if (dto.IsActive.HasValue)
            {
                vehicle.IsActive = dto.IsActive.Value;
            }

            await _vehicleRepository.UpdateAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();

            return vehicle;
        }
        public async Task<Vehicle?> GetVehicleByDriverIdAsync(Guid driverId)
        {
            return await _vehicleRepository.GetByDriverIdAsync(driverId);
        }
    }
}
