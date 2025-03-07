using System.Security.Claims;
using BusApp.DTOs;
using BusApp.Models;
using BusApp.Services.Implementations;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IClientService _clientService;

        public BookingController(IBookingService bookingService,
            IClientService clientService)
        {
            _bookingService = bookingService;
            _clientService = clientService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching bookings: {ex.Message}");
            }
        }

        // Get booking by Id
        [HttpGet("{bookingId}")]
        [Authorize(Roles = "Admin,TransportOperator,Client")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            try
            {
                var booking = await _bookingService.GetByIdAsync(bookingId);
                if (booking == null)
                    return NotFound("Booking not found.");

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the booking: {ex.Message}");
            }
        }

        // Get bookings by clientId
        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> GetBookingsByClient(int clientId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByClientIdAsync(clientId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching client bookings: {ex.Message}");
            }
        }

        // Add a new booking
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> AddBooking([FromBody] BookingDto bookingDto)
        {
            try
            {
                if (bookingDto == null)
                    return BadRequest("Invalid booking details.");

                // Get the authenticated user's email
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized("User email not found.");

                var newBooking = await _bookingService.AddBookingAsync(bookingDto, userEmail);
                if (newBooking == null)
                    return BadRequest("Failed to create booking.");

                return CreatedAtAction(nameof(GetBookingById), new { bookingId = newBooking.Id }, (object)newBooking); // Explicit cast to object
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the booking: {ex.Message}");
            }
        }

        // Cancel a booking
        [HttpPost("cancel/{bookingId}")]
        [Authorize(Roles = "Admin,Client,TransportOperator")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            try
            {
                bool isCancelled = await _bookingService.CancelBookingAsync(bookingId);
                if (!isCancelled)
                    return BadRequest("Cancellation failed or booking not found.");

                return Ok("Booking cancelled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while cancelling the booking: {ex.Message}");
            }
        }

        // Get available seats for a trip on a specific date
        [HttpGet("available-seats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableSeats([FromQuery] int tripId, [FromQuery] DateTime journeyDate)
        {
            try
            {
                int availableSeats = await _bookingService.GetAvailableSeatsAsync(tripId, journeyDate);
                return Ok(new { tripId, journeyDate, availableSeats });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching available seats: {ex.Message}");
            }
        }

        [HttpGet("search-trips")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchTrips([FromQuery] string source, [FromQuery] string destination, [FromQuery] DateTime journeyDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
                    return BadRequest("Source and destination are required.");

                var tripDetails = await _bookingService.SearchTripsAsync(source, destination, journeyDate);
                return Ok(tripDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while searching trips: {ex.Message}");
            }
        }

        [HttpGet("trip/{tripId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTripDetails(int tripId, [FromQuery] DateTime journeyDate)
        {
            try
            {
                var tripDetails = await _bookingService.GetTripDetailsAsync(tripId, journeyDate);
                if (tripDetails is null)
                    return NotFound("Trip not found.");

                TripSearchDetailsDto nonNullTripDetails = tripDetails; // Infer non-null type
                return Ok(nonNullTripDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching trip details: {ex.Message}");
            }
        }

        [HttpGet("mybookings")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetClientBookings()
        {
            try
            {
               
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized("User email not found in token.");

                
                var client = await _clientService.GetClientByEmailAsync(userEmail);
                if (client == null)
                    return Unauthorized("Client not found for this email.");

                var bookings = await _bookingService.GetBookingsByClientIdAsync(client.Id);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching your bookings: {ex.Message}");
            }
        }
    }
}
            
