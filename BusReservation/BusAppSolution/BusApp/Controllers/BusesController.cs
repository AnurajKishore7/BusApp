using BusApp.DTOs;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusesController : ControllerBase
    {
        private readonly IBusesService _busesService;

        public BusesController(IBusesService busesService)
        {
            _busesService = busesService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBuses()
        {
            try
            {
                var buses = await _busesService.GetAllBusesAsync();
                return Ok(buses);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllBuses (controller): {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving buses.");
            }
        }

        [HttpGet("my-buses")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> GetMyBuses()
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email)) return Unauthorized("Invalid user.");

                var buses = await _busesService.GetMyBusesAsync(email);
                return Ok(buses);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyBuses (controller): {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving your buses.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusById(int id)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role)) return Unauthorized("Invalid user.");

                var bus = await _busesService.GetBusByIdAsync(id, role, email);
                if (bus == null) return NotFound("Bus not found.");

                return Ok(bus);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBusById (controller): {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the bus.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> AddBus([FromBody] NewBusDto newBusDto)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email)) return Unauthorized("Invalid user.");

                var result = await _busesService.AddBusAsync(newBusDto, email);
                if (!result) return BadRequest("Failed to add bus.");

                return Ok("Bus added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBus (controller): {ex.Message}");
                return StatusCode(500, "An error occurred while adding the bus.");
            }
        }

        [HttpPut("{busId}")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> UpdateBus(int busId, [FromBody] UpdateBusDto updateBusDto)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email)) return Unauthorized("Invalid user.");

                var result = await _busesService.UpdateBusAsync(email, busId, updateBusDto);
                if (!result) return BadRequest("Failed to update bus.");

                return Ok("Bus updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBus (controller): {ex.Message}");
                return StatusCode(500, "An error occurred while updating the bus.");
            }
        }

        [HttpDelete("{busId}")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> DeleteBus(int busId)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email)) return Unauthorized("Invalid user.");

                var result = await _busesService.DeleteBusAsync(email, busId);
                if (!result) return BadRequest("Failed to delete bus.");

                return Ok("Bus deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteBus (controller): {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the bus.");
            }
        }
    }

}
