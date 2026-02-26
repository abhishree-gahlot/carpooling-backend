using Microsoft.AspNetCore.Mvc;
using CarpoolingSystem.Infrastructure.Data;
using CarpoolingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarpoolingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public VehicleController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddVehicle(Vehicle vehicle)
        {
            _dbContext.Add(vehicle);
            await _dbContext.SaveChangesAsync();
            return Ok(vehicle);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _dbContext.Vehicles.ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateVehicle(Guid id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return BadRequest();
            }

            _dbContext.Entry(vehicle).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(vehicle);
        }

        [HttpDelete]
        [Route("delete/{$id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _dbContext.Vehicles.Remove(vehicle);
            await _dbContext.SaveChangesAsync();
            return Ok("Vehicle deleted");
        }
    }
}
