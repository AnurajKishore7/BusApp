using BusApp.DTOs;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }

        [HttpGet("{id}/details")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTripDetails(int id, [FromQuery] DateTime journeyDate)
        {
            try
            {
                var trip = await _tripsService.GetTripDetailsAsync(id, journeyDate);
                if (trip == null)
                    return NotFound($"Trip with ID {id} not found for journey date {journeyDate:yyyy-MM-dd}");

                return Ok(trip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrips()
        {
            try
            {
                var trips = await _tripsService.GetAllTripsAsync();
                return Ok(trips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            try
            {
                var trip = await _tripsService.GetTripByIdAsync(id);
                if (trip == null)
                    return NotFound($"Trip with ID {id} not found.");

                return Ok(trip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("route/{busRouteId}")]
        public async Task<IActionResult> GetTripsByRoute(int busRouteId)
        {
            try
            {
                var trips = await _tripsService.GetTripsByRouteAsync(busRouteId);
                return Ok(trips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTripsBySourceAndDestination([FromQuery] string source, [FromQuery] string destination)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
                    return BadRequest("Source and destination are required.");

                var trips = await _tripsService.GetTripsBySourceAndDestinationAsync(source, destination);
                return Ok(trips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles ="TransportOperator")]
        public async Task<IActionResult> AddTrip([FromBody] TripDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var addedTrip = await _tripsService.AddTripAsync(dto);
                if (addedTrip == null)
                    return StatusCode(500, "Failed to add trip.");

                return CreatedAtAction(nameof(GetTripById), new { id = addedTrip.Id }, addedTrip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] TripDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedTrip = await _tripsService.UpdateTripAsync(id, dto);
                if (updatedTrip == null)
                    return NotFound($"Trip with ID {id} not found or could not be updated.");

                return Ok(updatedTrip);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            try
            {
                var result = await _tripsService.DeleteTripAsync(id);
                if (!result)
                    return NotFound($"Trip with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
