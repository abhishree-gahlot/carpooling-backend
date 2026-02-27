using AutoMapper;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Services;
using CarpoolingSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarpoolingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Driver))]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController(VehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleCreateDTO vehicleDto)
        {
            try
            {
                var userId = Guid.Parse(User.Identity!.Name!);
                var roleClaim = User.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;
                var userRole = Enum.Parse<UserRole>(roleClaim ?? "Passenger");

                var vehicle = await _vehicleService.CreateVehicleAsync(vehicleDto, userRole, userId);

                var resultDto = _mapper.Map<VehicleDTO>(vehicle);
                return Ok(resultDto);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetAllVehicles()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            var vehicleDtos = _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);
            return Ok(vehicleDtos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<VehicleDTO>> GetVehicleById(Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VehicleDTO>(vehicle));
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] VehicleUpdateDTO vehicleDto)
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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            try
            {
                await _vehicleService.DeleteVehicleAsync(id);
                return Ok("Vehicle deleted");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}