using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarpoolingSystem.API.Controller
{
    [Route("api/carpool")]
    [ApiController]
    [Authorize]
    public class CarpoolController : ControllerBase
    {
        private readonly ICarpoolService _carpoolService;

        public CarpoolController(ICarpoolService carpoolService)
        {
            _carpoolService = carpoolService;
        }

        // GET /api/carpool/session/{sessionId}/status
        [HttpGet("session/{sessionId}/status")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> GetSessionStatus(Guid sessionId)
        {
            try
            {
                var result = await _carpoolService.GetSessionStatusAsync(sessionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/carpool/session/{sessionId}/pending-requests
        [HttpGet("session/{sessionId}/pending-requests")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> GetPendingRequests(Guid sessionId)
        {
            try
            {
                var result = await _carpoolService.GetPendingRequestsForSessionAsync(sessionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/carpool/accept
        // Body: { sessionId, requestId }
        [HttpPost("accept")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> AcceptRequest([FromBody] AcceptRequestDto dto)
        {
            try
            {
                var booking = await _carpoolService.AcceptRequestAsync(dto.SessionId, dto.RequestId);
                return Ok(new
                {
                    bookingId = booking.BookingId,
                    pin = booking.PIN,
                    status = booking.Status.ToString()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/carpool/verify-pin
        // Body: { bookingId, enteredPin }
        [HttpPost("verify-pin")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> VerifyPin([FromBody] VerifyPinDto dto)
        {
            try
            {
                var success = await _carpoolService.VerifyPinAsync(dto.BookingId, dto.EnteredPin);
                if (!success)
                    return BadRequest(new { message = "Invalid PIN. Please try again." });

                return Ok(new { message = "PIN verified. Passenger boarded." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/carpool/complete
        // Body: { sessionId }
        [HttpPost("complete")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> CompleteJourney([FromBody] CompleteJourneyDto dto)
        {
            try
            {
                await _carpoolService.CompleteJourneyAsync(dto.SessionId);
                return Ok(new { message = "Journey completed. Session closed." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    // Request DTOs (inline — move to Application/DTOs if preferred)
    public record AcceptRequestDto(Guid SessionId, Guid RequestId);
    public record VerifyPinDto(Guid BookingId, string EnteredPin);
    public record CompleteJourneyDto(Guid SessionId);
}