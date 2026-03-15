namespace CarpoolingSystem.Application.Interfaces;

public interface IHubService
{
    Task NotifyDriverAsync(Guid userId, string eventName, object data);
    Task NotifyPassengerAsync(Guid userId, string eventName, object data);
    Task NotifyGroupAsync(string groupName, string eventName, object data);
}