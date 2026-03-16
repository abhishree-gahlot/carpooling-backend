using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarpoolingSystem.Infrastructure.Hubs;

[Authorize]
public class RideHub : Hub
{
    private readonly IHubService _hubService;
    private readonly IRideRequestService _rideRequestService;

    public RideHub(IHubService hubService, IRideRequestService rideRequestService)
    {
        _hubService = hubService;
        _rideRequestService = rideRequestService;
    }

    public async Task NotifyDriver(NotifyDriverDto dto)
    {
        var rideRequest = await _rideRequestService.GetRequestByIdAsync(dto.RideRequestId);
        var passengerName = rideRequest?.Passenger?.UserName ?? "Passenger";

        Console.WriteLine($"[RideHub] NotifyDriver | DriverId={dto.DriverId} RequestId={dto.RideRequestId}");

        await _hubService.NotifyDriverAsync(dto.DriverId, "NewRideRequest", new
        {
            rideRequestId = dto.RideRequestId,
            passengerName = passengerName,
            pickupName = dto.PickupName,
            pickupLat = dto.PickupLat,
            pickupLng = dto.PickupLng,
            destinationName = dto.DestinationName
        });
    }

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
            Console.WriteLine($"[RideHub] Total active connections: {ConnectionStore.GetAllConnections()}");
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