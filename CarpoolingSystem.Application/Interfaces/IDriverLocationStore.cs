using CarpoolingSystem.Application.DTOs;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface IDriverLocationStore
    {
        void UpdateLocation(Guid driverId, double latitude, double longitude);
        IEnumerable<DriverLocationDto> GetAllLocations();
        bool TryRemove(Guid driverId);
    }
}
