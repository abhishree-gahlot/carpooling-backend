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
        var connectionId = ConnectionStore.GetConnectionId(driverUserId);

        if (connectionId is not null)
            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(eventName, data);
    }

    public async Task NotifyPassengerAsync(Guid passengerUserId, string eventName, object data)
    {
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