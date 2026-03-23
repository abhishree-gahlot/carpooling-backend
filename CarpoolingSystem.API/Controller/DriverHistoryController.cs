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
    public class DriverHistoryController : ControllerBase {

        private readonly IDriverHistoryService _driverHistoryService;
        private readonly IDriverHistoryPassengerService _passengerService;
        private readonly IMapper _mapper;

        public DriverHistoryController(IDriverHistoryService driverHistoryService, IDriverHistoryPassengerService passengerService, IMapper mapper) {
            _driverHistoryService = driverHistoryService;
            _passengerService = passengerService;
            _mapper = mapper;
        }

        [HttpPost("create-from-session/{sessionId}")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> CreateFromSession(Guid sessionId) {
            try {
                var history = await _driverHistoryService.CreateFromSessionAsync(sessionId);
                return Ok(_mapper.Map<DriverHistoryDto>(history));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll() {
            try {
                var histories = await _driverHistoryService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<DriverHistoryDto>>(histories));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            try {
                var history = await _driverHistoryService.GetByIdAsync(id);

                if (history == null)
                    return NotFound("Driver history not found.");

                return Ok(_mapper.Map<DriverHistoryDto>(history));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("driver/myhistory")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> GetMyHistory() {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid driverId = Guid.Parse(userIdClaim);
                var histories = await _driverHistoryService.GetByDriverIdAsync(driverId);
                return Ok(_mapper.Map<IEnumerable<DriverHistoryDto>>(histories));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("driver/{driverId}")]
        public async Task<IActionResult> GetByDriverId(Guid driverId) {
            try {
                var histories = await _driverHistoryService.GetByDriverIdAsync(driverId);
                return Ok(_mapper.Map<IEnumerable<DriverHistoryDto>>(histories));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> Delete(Guid id) {
            try {
                await _driverHistoryService.DeleteAsync(id);
                return Ok(new { message = "Driver history deleted successfully." });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("passenger/myrides")]
        [Authorize(Roles = nameof(UserRole.Passenger))]
        public async Task<IActionResult> GetMyRides() {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid passengerId = Guid.Parse(userIdClaim);
                var entries = await _passengerService.GetByPassengerIdAsync(passengerId);
                return Ok(_mapper.Map<IEnumerable<DriverHistoryPassengerDto>>(entries));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("passenger/rate-driver/{passengerHistoryId}")]
        [Authorize(Roles = nameof(UserRole.Passenger))]
        public async Task<IActionResult> RateDriver(
            Guid passengerHistoryId,
            [FromBody] RateDriverDto dto) {
            try {
                var entry = await _passengerService.RateDriverAsync(passengerHistoryId, dto);
                return Ok(_mapper.Map<DriverHistoryPassengerDto>(entry));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
