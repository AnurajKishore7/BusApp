using BusApp.DTOs;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorService _operatorService;

        public OperatorController(IOperatorService operatorService)
        {
            _operatorService = operatorService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOperators()
        {
            try
            {
                var operators = await _operatorService.GetAllOperatorsAsync();
                return Ok(operators);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllOperators: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOperatorById(int id)
        {
            try
            {
                var transportOperator = await _operatorService.GetOperatorByIdAsync(id);
                if (transportOperator == null) return NotFound($"Transport Operator with {id} not found.");
                return Ok(transportOperator);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOperatorById: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOperatorByEmail(string email)
        {
            try
            {
                var transportOperator = await _operatorService.GetOperatorByEmailAsync(email);
                if (transportOperator == null) return NotFound($"Transport Operator with {email} not found.");
                return Ok(transportOperator);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOperatorByEmail: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("profile")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> GetOperatorProfile()
        {
            try
            {
                var transportOperator = await _operatorService.GetOperatorAsync();
                if (transportOperator == null) return NotFound("Transport Operator profile not found.");
                return Ok(transportOperator);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOperatorProfile: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> UpdateOperator([FromBody] UpdateOperatorDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _operatorService.UpdateOperatorAsync(updateDto);
                if (!result) return BadRequest("Failed to update transport operator.");

                return Ok("Transport Operator updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateOperator: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "TransportOperator")]
        public async Task<IActionResult> DeleteOperator()
        {
            try
            {
                var result = await _operatorService.DeleteOperatorAsync();
                if (!result) return BadRequest("Failed to delete transport operator.");

                return Ok("Transport Operator deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteOperator: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOperatorByEmail(string email)
        {
            try
            {
                var result = await _operatorService.DeleteOperatorAsync(email);
                if (!result) return BadRequest("Failed to delete transport operator.");

                return Ok("Transport Operator deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteOperatorByEmail: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
