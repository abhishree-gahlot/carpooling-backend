using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarpoolingSystem.API.Controller {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RideRequestController : ControllerBase {
        private readonly IRideRequestService _rideRequestService;
        private readonly IMapper _mapper;
        private readonly IHubService _hubService;

        public RideRequestController(
            IRideRequestService rideRequestService,
            IMapper mapper,
            IHubService hubService
        ) {
            _rideRequestService = rideRequestService;
            _mapper = mapper;
            _hubService = hubService;
        }

        [HttpPost("create")]
        [Authorize(Roles = nameof(UserRole.Passenger))]
        public async Task<IActionResult> CreateRequest([FromBody] RideRequestCreateDto dto) {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid passengerId = Guid.Parse(userIdClaim);

                var request = await _rideRequestService
                    .CreateRequestAsync(dto, passengerId);

                return Ok(_mapper.Map<RideRequestDto>(request));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> UpdateRequest(
            Guid id, [FromBody] RideRequestUpdateDto dto) {
            try {
                var request = await _rideRequestService.UpdateRequestAsync(id, dto);
                return Ok(_mapper.Map<RideRequestDto>(request));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("cancel/{id}")]
        [Authorize(Roles = nameof(UserRole.Passenger))]
        public async Task<IActionResult> CancelRequest(Guid id) {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid passengerId = Guid.Parse(userIdClaim);

                var request = await _rideRequestService
                    .CancelRequestAsync(id, passengerId);

                return Ok(_mapper.Map<RideRequestDto>(request));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRequests() {
            try {
                var requests = await _rideRequestService.GetAllRequestsAsync();
                return Ok(_mapper.Map<IEnumerable<RideRequestDto>>(requests));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(Guid id) {
            try {
                var request = await _rideRequestService.GetRequestByIdAsync(id);

                if (request == null)
                    return NotFound("Ride request not found.");

                return Ok(_mapper.Map<RideRequestDto>(request));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("passenger/myrequests")]
        [Authorize(Roles = nameof(UserRole.Passenger))]
        public async Task<IActionResult> GetMyRequests() {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid passengerId = Guid.Parse(userIdClaim);

                var requests = await _rideRequestService
                    .GetRequestsByPassengerIdAsync(passengerId);

                return Ok(_mapper.Map<IEnumerable<RideRequestDto>>(requests));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRequest(Guid id) {
            try {
                await _rideRequestService.DeleteRequestAsync(id);
                return Ok(new { message = "Ride request deleted successfully." });
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> GetPendingRequests() {
            try {
                var requests = await _rideRequestService.GetPendingRequestsAsync();
                return Ok(_mapper.Map<IEnumerable<RideRequestDto>>(requests));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("{requestId}/notify-driver/{driverId}")]
        [Authorize(Roles = nameof(UserRole.Passenger))]
        public async Task<IActionResult> NotifyDriver(Guid requestId, Guid driverId)
        {
            try
            {
                var request = await _rideRequestService.GetRequestByIdAsync(requestId);

                if (request == null)
                    return NotFound("Ride request not found.");

                await _hubService.NotifyDriverAsync(driverId, "NewRideRequest", new
                {
                    requestId = request.Id,
                    passengerName = request.Passenger?.UserName ?? "Passenger",
                    passengerId = request.PassengerId,
                    pickup = request.PickupName,
                    destination = request.DestinationName
                });

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
