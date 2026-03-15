using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicle> CreateVehicleAsync(VehicleCreateDTO dto, UserRole role, Guid userId);
        Task<Vehicle> UpdateVehicleAsync(Guid vehicleId, VehicleUpdateDTO dto);
        Task<Vehicle?> GetVehicleByDriverIdAsync(Guid driverId);
    }
}
