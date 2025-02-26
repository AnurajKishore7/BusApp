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

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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
                var success = await _paymentService.UpdatePaymentStatusAsync(paymentId, "Completed");
                if (!success) return BadRequest("Failed to confirm payment.");

                return Ok("Payment confirmed and booking marked as confirmed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while confirming the payment: {ex.Message}");
            }
        }

    }

}
