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
    public class BookingController : ControllerBase {

        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(IBookingService bookingService, IMapper mapper) {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        //[HttpPost("create")]
        //[Authorize(Roles = nameof(UserRole.Passenger))]
        //public async Task<IActionResult> CreateBooking(
        //    [FromBody] BookingCreateDto dto) {
        //    try {
        //        var booking = await _bookingService.CreateBookingAsync(dto);
        //        return Ok(_mapper.Map<BookingDto>(booking));
        //    }
        //    catch (Exception exception) {
        //        return BadRequest(exception.Message);
        //    }
        //}

        [HttpPost("accept/{rideRequestId}/{sessionId}")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> AcceptBooking(Guid rideRequestId, Guid sessionId)
        {
            try
            {
                var booking = await _bookingService.AcceptBookingAsync(rideRequestId, sessionId);
                return Ok(_mapper.Map<BookingDto>(booking));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("verifypin/{id}")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> VerifyPin(
            Guid id, [FromBody] BookingVerifyPinDto dto) {
            try {
                var booking = await _bookingService.VerifyPinAsync(id, dto);
                return Ok(_mapper.Map<BookingDto>(booking));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("complete/{id}")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> CompleteBooking(Guid id) {
            try {
                var booking = await _bookingService.CompleteBookingAsync(id);
                return Ok(_mapper.Map<BookingDto>(booking));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("cancel/{id}")]
        [Authorize(Roles = nameof(UserRole.Passenger))]
        public async Task<IActionResult> CancelBooking(Guid id) {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid passengerId = Guid.Parse(userIdClaim);
                var booking = await _bookingService
                    .CancelBookingAsync(id, passengerId);

                return Ok(_mapper.Map<BookingDto>(booking));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookings() {
            try {
                var bookings = await _bookingService.GetAllBookingsAsync();
                return Ok(_mapper.Map<IEnumerable<BookingDto>>(bookings));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id) {
            try {
                var booking = await _bookingService.GetBookingByIdAsync(id);

                if (booking == null)
                    return NotFound("Booking not found.");

                return Ok(_mapper.Map<BookingDto>(booking));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("passenger/mybookings")]
        [Authorize(Roles = nameof(UserRole.Passenger))]
        public async Task<IActionResult> GetMyBookings() {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid passengerId = Guid.Parse(userIdClaim);
                var bookings = await _bookingService
                    .GetBookingsByPassengerIdAsync(passengerId);

                return Ok(_mapper.Map<IEnumerable<BookingDto>>(bookings));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("driver/mybookings")]
        [Authorize(Roles = nameof(UserRole.Driver))]
        public async Task<IActionResult> GetDriverBookings() {
            try {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                Guid driverId = Guid.Parse(userIdClaim);
                var bookings = await _bookingService
                    .GetBookingsByDriverIdAsync(driverId);

                return Ok(_mapper.Map<IEnumerable<BookingDto>>(bookings));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetBookingsBySession(Guid sessionId) {
            try {
                var bookings = await _bookingService
                    .GetBookingsBySessionIdAsync(sessionId);

                return Ok(_mapper.Map<IEnumerable<BookingDto>>(bookings));
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id) {
            try {
                await _bookingService.DeleteBookingAsync(id);
                return Ok(new { message = "Booking deleted successfully." });
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

    }
}
