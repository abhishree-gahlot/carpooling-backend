using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Application.Services;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarpoolingSystem.API.Controller {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RideSessionController : ControllerBase {
        private readonly IRideSessionService _rideSessionService;
        private readonly IMapper _mapper;

        public RideSessionController(IRideSessionService rideSessionService, IMapper mapper) {
            _rideSessionService= rideSessionService;
            _mapper = mapper;
            
        }

        [HttpPost("start")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> StartSession(
            [FromBody] RideSessionCreateDto dto) {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid driverId = Guid.Parse(userIdClaim);

                var session = await _rideSessionService
                    .CreateSessionAsync(dto, driverId);

                return Ok(_mapper.Map<RideSessionDto>(session));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("stop")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> StopSession() {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid driverId = Guid.Parse(userIdClaim);

                var session = await _rideSessionService.GoOfflineAsync(driverId);

                return Ok(_mapper.Map<RideSessionDto>(session));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> UpdateSession(
            Guid id, [FromBody] RideSessionUpdateDto dto) {
            try {
                var session = await _rideSessionService.UpdateSessionAsync(id, dto);
                return Ok(_mapper.Map<RideSessionDto>(session));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSessions() {
            try {
                var sessions = await _rideSessionService.GetAllSessionsAsync();
                return Ok(_mapper.Map<IEnumerable<RideSessionDto>>(sessions));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(Guid id) {
            try {
                var session = await _rideSessionService.GetSessionByIdAsync(id);

                if (session == null)
                    return NotFound("Ride session not found.");

                return Ok(_mapper.Map<RideSessionDto>(session));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> DeleteSession(Guid id) {
            try {
                await _rideSessionService.DeleteSessionAsync(id);
                return Ok(new { message = "Ride session deleted successfully." });
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("driver/mysessions")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> GetMySessions() {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid driverId = Guid.Parse(userIdClaim);

                var sessions = await _rideSessionService
                    .GetSessionsByDriverIdAsync(driverId);

                return Ok(_mapper.Map<IEnumerable<RideSessionDto>>(sessions));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }
    }
}
