using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;

namespace CarpoolingSystem.Application.Services
{
    public class RideRequestService : IRideRequestService
    {

        private readonly IRideRequestRepository _rideRequestRepository;
        private readonly IHubService _hubService;

        public RideRequestService(
            IRideRequestRepository rideRequestRepository,
            IHubService hubService)
        {
            _rideRequestRepository = rideRequestRepository;
            _hubService = hubService;
        }

        public async Task<RideRequests> CreateRequestAsync(
            RideRequestCreateDto dto, Guid passengerId)
        {

            var request = new RideRequests
            {
                Id = Guid.NewGuid(),
                PassengerId = passengerId,
                PickupLatitude = dto.Pickup.Latitude,
                PickupLongitude = dto.Pickup.Longitude,
                PickupName = dto.Pickup.Name,
                DestinationLatitude = dto.Destination.Latitude,
                DestinationLongitude = dto.Destination.Longitude,
                DestinationName = dto.Destination.Name,
                RideRequestStatus = RideRequestStatus.Pending,
                RequestedAt = DateTime.UtcNow,
                RespondedAt = null
            };

            await _rideRequestRepository.AddAsync(request);
            await _rideRequestRepository.SaveChangesAsync();

            var created = await _rideRequestRepository.GetByIdAsync(request.Id)
                ?? throw new Exception("Failed to retrieve created request.");

            return created;
        }

        public async Task<RideRequests> UpdateRequestAsync(
            Guid requestId, RideRequestUpdateDto dto)
        {
            var request = await _rideRequestRepository.GetByIdAsync(requestId); // this fetched row is updating later on the database so dont use notracking here

            if (request == null)
                throw new Exception("Ride request not found.");

            if (dto.RideRequestStatus.HasValue)
            {
                request.RideRequestStatus = dto.RideRequestStatus.Value;
                request.RespondedAt = DateTime.UtcNow;
                // update seats here total and avaiable in the request object
            }

            _rideRequestRepository.Update(request);
            await _rideRequestRepository.SaveChangesAsync();

            var eventName = dto.RideRequestStatus == RideRequestStatus.Accepted
                ? "RideAccepted" : "RideRejected";

            await _hubService.NotifyPassengerAsync(
                request.PassengerId, eventName, new
                {
                    requestId = request.Id,
                    status = request.RideRequestStatus.ToString()
                });

            return request;
        }

        public async Task<RideRequests> CancelRequestAsync(
            Guid requestId, Guid passengerId)
        {

            var request = await _rideRequestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new Exception("Ride request not found.");

            if (request.PassengerId != passengerId)
                throw new Exception("You can only cancel your own requests.");

            if (request.RideRequestStatus != RideRequestStatus.Pending)
                throw new Exception("Only pending requests can be cancelled.");

            request.RideRequestStatus = RideRequestStatus.Cancelled;
            request.RespondedAt = DateTime.UtcNow;

            _rideRequestRepository.Update(request);
            await _rideRequestRepository.SaveChangesAsync();

            return request;
        }

        public async Task<bool> DeleteRequestAsync(Guid requestId)
        {
            var request = await _rideRequestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new Exception("Ride request not found.");

            _rideRequestRepository.Delete(request);
            return await _rideRequestRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<RideRequests>> GetAllRequestsAsync() =>
            await _rideRequestRepository.GetallAsync();

        public async Task<RideRequests?> GetRequestByIdAsync(Guid requestId) =>
            await _rideRequestRepository.GetByIdAsync(requestId);

        public async Task<IEnumerable<RideRequests>> GetRequestsByPassengerIdAsync(
            Guid passengerId) =>
            await _rideRequestRepository.GetByPassengerIdAsync(passengerId);

        public async Task<IEnumerable<RideRequests>> GetPendingRequestsAsync() =>
            await _rideRequestRepository.GetPendingRequestsAsync();
    }
}