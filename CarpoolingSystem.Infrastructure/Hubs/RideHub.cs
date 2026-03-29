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
            sessionId = dto.SessionId,
            passengerName = passengerName,
            pickupName = dto.PickupName,
            pickupLat = dto.PickupLat,
            pickupLng = dto.PickupLng,
            destinationName = dto.DestinationName,
            destinationLat = rideRequest?.DestinationLatitude,  
            destinationLng = rideRequest?.DestinationLongitude  
        });
    }

    public async Task NotifyDriverNewPassengerRequest(NotifyDriverNewPassengerRequestDto dto)
    {
        Console.WriteLine($"[RideHub] NotifyDriverNewPassengerRequest | DriverId={dto.DriverId} RequestId={dto.RequestId}");

        await _hubService.NotifyDriverAsync(dto.DriverId, "NewPassengerRequest", new
        {
            requestId = dto.RequestId,
            passengerId = dto.PassengerId,
            passengerName = dto.PassengerName,
            pickupName = dto.PickupName,
            pickupLatitude = dto.PickupLatitude,
            pickupLongitude = dto.PickupLongitude,
            destinationName = dto.DestinationName,
            requestedAt = DateTime.UtcNow
        });
    }

    public async Task NotifySeatCountUpdated(NotifySeatCountDto dto)
    {
        Console.WriteLine($"[RideHub] NotifySeatCountUpdated | SessionId={dto.SessionId} Available={dto.AvailableSeats}/{dto.TotalSeats}");

        await Clients.Group(dto.SessionId.ToString()).SendAsync("SeatCountUpdated", new
        {
            sessionId = dto.SessionId,
            availableSeats = dto.AvailableSeats,
            totalSeats = dto.TotalSeats
        });
    }

    public async Task NotifyPassengerRequestAccepted(NotifyPassengerRequestAcceptedDto dto)
    {
        Console.WriteLine($"[RideHub] NotifyPassengerRequestAccepted | PassengerId={dto.PassengerId} BookingId={dto.BookingId}");

        await _hubService.NotifyPassengerAsync(dto.PassengerId, "RideAccepted", new
        {
            bookingId = dto.BookingId,
            requestId = dto.RequestId
        });
    }

    public override async Task OnConnectedAsync()
    {
        var userIdString = Context.User?
        .FindFirst(JwtRegisteredClaimNames.Sub)?.Value
        ?? Context.User?
        .FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
            Console.WriteLine($"[RideHub] All connections after add: {ConnectionStore.GetAllConnections()}");
        }
        else
        {
            Console.WriteLine($"[RideHub] WARNING: Could not parse userId from token. Raw value: {userIdString}");
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
            Console.WriteLine($"[RideHub] All connections after remove: {ConnectionStore.GetAllConnections()}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task NotifyPassengerRejected(NotifyPassengerRejectedDto dto)
    {
        Console.WriteLine($"[RideHub] NotifyPassengerRejected | PassengerId={dto.PassengerId}");

        await _hubService.NotifyPassengerAsync(dto.PassengerId, "RideRejected", new
        {
            rideRequestId = dto.RideRequestId,
            reason = "Driver accepted another passenger"
        });
    }
}