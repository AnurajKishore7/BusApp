using BusApp.DTOs;
using BusApp.Services.Implementations;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;

        public PaymentController(IPaymentService paymentService,
            IBookingService bookingService)
        {
            _paymentService = paymentService;
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var payments = await _paymentService.GetAllAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching payments: {ex.Message}");
            }
        }

        [HttpGet("{paymentId}")]
        [Authorize]
        public async Task<IActionResult> GetById(int paymentId)
        {
            try
            {
                var payment = await _paymentService.GetByIdAsync(paymentId);
                if (payment == null) return NotFound("Payment not found.");
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the payment: {ex.Message}");
            }
        }

        [HttpGet("booking/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> GetByBookingId(int bookingId)
        {
            try
            {
                var payment = await _paymentService.GetByBookingIdAsync(bookingId);
                if (payment == null) return NotFound("Payment not found for this booking.");
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the payment by booking ID: {ex.Message}");
            }
        }

        [HttpPost("confirm-payment/{paymentId}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ConfirmPayment(int paymentId)
        {
            try
            {
                var payment = await _paymentService.GetByIdAsync(paymentId);
                if (payment == null) return NotFound("Payment not found.");

                var booking = await _bookingService.GetByIdAsync(payment.BookingId);
                if (booking == null) return BadRequest("Associated booking not found.");

                decimal ticketPrice = booking.TotalAmount;
                var success = await _paymentService.UpdatePaymentStatusAsync(paymentId, "Completed", ticketPrice);
                if (!success) return BadRequest("Failed to confirm payment.");

                var response = new PaymentResponseDto
                {
                    Id = payment.Id,
                    BookingId = payment.BookingId,
                    TotalAmount = payment.TotalAmount,
                    PaymentMethod = payment.PaymentMethod,
                    Status = "Completed"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while confirming the payment: {ex.Message}");
            }
        }

        [HttpPut("update-method/{paymentId}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> UpdatePaymentMethod(int paymentId, [FromBody] UpdatePaymentMethodDto dto)
        {
            try
            {
                var success = await _paymentService.UpdatePaymentMethodAsync(paymentId, dto.PaymentMethod);
                if (!success) return BadRequest("Failed to update payment method.");

                return Ok("Payment method updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the payment method: {ex.Message}");
            }
        }

    }

}
