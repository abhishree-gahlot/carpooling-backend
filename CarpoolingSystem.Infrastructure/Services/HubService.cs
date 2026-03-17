using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CarpoolingSystem.Infrastructure.Services;

public class HubService : IHubService
{
    private readonly IHubContext<RideHub> _hubContext;

    public HubService(IHubContext<RideHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyDriverAsync(Guid driverUserId, string eventName, object data)
    {
        Console.WriteLine($"[ConnectionStore] All keys: {ConnectionStore.GetAllConnections()}");
        Console.WriteLine($"[ConnectionStore] Looking up: {driverUserId}");

        var connectionId = ConnectionStore.GetConnectionId(driverUserId);
        Console.WriteLine($"[HubService] NotifyDriver | DriverId={driverUserId} ConnectionId={connectionId ?? "NOT FOUND"}");

        if (connectionId is not null)
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(eventName, data);
    }

    public async Task NotifyPassengerAsync(Guid passengerUserId, string eventName, object data)
    {
        Console.WriteLine($"[HubService] NotifyPassenger | PassengerId={passengerUserId}");

        var connectionId = ConnectionStore.GetConnectionId(passengerUserId);

        if (connectionId is not null)
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(eventName, data);
    }

    public async Task NotifyGroupAsync(string groupName, string eventName, object data)
    {
        await _hubContext.Clients
            .Group(groupName)
            .SendAsync(eventName, data);
    }
}