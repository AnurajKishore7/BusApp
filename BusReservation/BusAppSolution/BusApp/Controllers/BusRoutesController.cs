using BusApp.DTOs;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusRoutesController : ControllerBase
    {
        private readonly IBusRoutesService _busRoutesService;

        public BusRoutesController(IBusRoutesService busRoutesService)
        {
            _busRoutesService = busRoutesService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,TransportOperator")]
        public async Task<IActionResult> GetAllBusRoutes()
        {
            try
            {
                var busRoutes = await _busRoutesService.GetAllBusRoutesAsync();
                return Ok(busRoutes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Admin,TransportOperator")]
        public async Task<IActionResult> GetBusRouteById(int id)
        {
            try
            {
                var busRoute = await _busRoutesService.GetBusRouteByIdAsync(id);
                if (busRoute == null)
                    return NotFound("Bus route not found.");

                return Ok(busRoute);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchBusRoute([FromQuery] string source, [FromQuery] string destination)
        {
            try
            {
                var busRoute = await _busRoutesService.SearchBusRouteAsync(source, destination);
                if (busRoute == null)
                    return NotFound("Bus route not found.");

                return Ok(busRoute);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBusRoute([FromBody] BusRouteDto newBusRouteDto)
        {
            try
            {
                var createdBusRoute = await _busRoutesService.AddBusRouteAsync(newBusRouteDto);
                if (createdBusRoute == null)
                    return StatusCode(500, "Failed to add bus route.");

                return CreatedAtAction(nameof(GetBusRouteById), new { id = createdBusRoute.Id }, createdBusRoute);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBusRoute(int id, [FromBody] BusRouteDto updatedBusRouteDto)
        {
            try
            {
                var result = await _busRoutesService.UpdateBusRouteAsync(id, updatedBusRouteDto);
                if (!result)
                    return StatusCode(500, "Failed to update bus route.");

                return NoContent(); // 204 No Content for successful updates
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBusRoute(int id)
        {
            try
            {
                var result = await _busRoutesService.DeleteBusRouteAsync(id);
                if (!result)
                    return NotFound("Bus route not found or already deleted.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
