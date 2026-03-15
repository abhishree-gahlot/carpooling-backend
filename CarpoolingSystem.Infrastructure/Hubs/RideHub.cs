using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarpoolingSystem.Infrastructure.Hubs;

[Authorize]
public class RideHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userIdString = Context.User?
            .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        var role = Context.User?
            .FindFirst(ClaimTypes.Role)?.Value;

        if (Guid.TryParse(userIdString, out Guid userId))
        {
            ConnectionStore.Add(userId, Context.ConnectionId);

            if (role == "Driver")
                await Groups.AddToGroupAsync(Context.ConnectionId, "Drivers");
            else
                await Groups.AddToGroupAsync(Context.ConnectionId, "Passengers");

            Console.WriteLine($"[RideHub] Connected | UserId={userId} Role={role} ConnId={Context.ConnectionId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userIdString = Context.User?
            .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (Guid.TryParse(userIdString, out Guid userId))
        {
            ConnectionStore.Remove(userId);
            Console.WriteLine($"[RideHub] Disconnected | UserId={userId}");
        }

        await base.OnDisconnectedAsync(exception);
    }
}