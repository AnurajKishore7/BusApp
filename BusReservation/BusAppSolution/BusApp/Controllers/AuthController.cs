using BusApp.DTOs;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register-client")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRegisterDto clientDto)
        {
            try
            {
                var result = await _authService.RegisterClient(clientDto);
                if (result == null)
                    return BadRequest(new { message = "Email already exists" });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("register-transport-operator")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterTransportOperator([FromBody] TransportRegisterDto operatorDto)
        {
            try
            {
                var result = await _authService.RegisterTransportOperator(operatorDto);
                if (result == null)
                    return BadRequest(new { message = "Email already exists" });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.Login(loginDto);
                if (result == null)
                    return Unauthorized(new { message = "Invalid credentials" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("pending-operators")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetPendingOperators()
        {
            try
            {
                var pendingOperators = await _authService.GetPendingOperators();
                return Ok(pendingOperators);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  new { message = "An error occurred while fetching pending operators.", error = ex.Message });
            }
        }

        [HttpPut("approve-operator/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveTransportOperator(string name)
        {
            try
            {
                var result = await _authService.ApproveTransportOperator(name);
                if (!result)
                    return NotFound(new { message = "Transport operator not found or already approved" });

                return Ok(new { message = "Transport operator approved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
    }
}
