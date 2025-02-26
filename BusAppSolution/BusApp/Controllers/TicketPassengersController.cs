using BusApp.DTOs;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketPassengersController : ControllerBase
    {
        private readonly ITicketPassengerService _ticketPassengerService;

        public TicketPassengersController(ITicketPassengerService ticketPassengerService)
        {
            _ticketPassengerService = ticketPassengerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketPassengerResponseDto>>> GetAll()
        {
            try
            {
                var passengers = await _ticketPassengerService.GetAllAsync();
                return Ok(passengers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll (Controller): {ex.Message}");
                return StatusCode(500, "An error occurred while fetching passengers.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketPassengerResponseDto>> GetById(int id)
        {
            try
            {
                var passenger = await _ticketPassengerService.GetByIdAsync(id);
                if (passenger == null)
                    return NotFound($"No passenger found with ID {id}.");

                return Ok(passenger);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById (Controller) for ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the passenger.");
            }
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<IEnumerable<TicketPassengerResponseDto>>> GetByBookingId(int bookingId)
        {
            try
            {
                var passengers = await _ticketPassengerService.GetByBookingIdAsync(bookingId);
                return Ok(passengers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByBookingId (Controller) for BookingId {bookingId}: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching passengers for the booking.");
            }
        }
    }

}
