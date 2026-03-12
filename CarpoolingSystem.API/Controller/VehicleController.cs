using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarpoolingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Driver))]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController( IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleCreateDTO vehicleDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("User ID not found in token");
                }

                Guid userId = Guid.Parse(userIdClaim);

                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(roleClaim))
                {
                    return Unauthorized("User role not found in token");
                }

                var userRole = Enum.Parse<UserRole>(roleClaim);

                var vehicle = await _vehicleService.CreateVehicleAsync(
                    vehicleDto,
                    userRole,
                    userId
                );

                var result = _mapper.Map<VehicleDTO>(vehicle);

                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateVehicle(Guid id,[FromBody] VehicleUpdateDTO vehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.UpdateVehicleAsync(id, vehicleDto);

                return Ok(_mapper.Map<VehicleDTO>(vehicle));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}