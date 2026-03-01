//using CarpoolingSystem.Domain.Entities;
//using CarpoolingSystem.Application.DTOs;
//using CarpoolingSystem.Domain.Enums;

//namespace CarpoolingSystem.Application.Interfaces
//{
//    public interface IVehicleService
//    {
//        Task<Vehicle?> GetVehicleByIdAsync(Guid id);
//        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
//        Task<Vehicle> CreateVehicleAsync(VehicleCreateDTO dto, UserRole userRole, Guid currentUserId);
//        Task<Vehicle> UpdateVehicleAsync(Guid id, VehicleUpdateDTO dto);
//        Task DeleteVehicleAsync(Guid id);
//    }
//}