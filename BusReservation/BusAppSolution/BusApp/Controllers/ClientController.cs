using BusApp.DTOs;
using BusApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var clients = await _clientService.GetAllClientsAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllClients: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,TransportOperator")]
        public async Task<IActionResult> GetClientById(int id)
        {
            try
            {
                var client = await _clientService.GetClientByIdAsync(id);
                if (client == null) return NotFound($"Client with {id} not found.");
                return Ok(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientById: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetClientByEmail(string email)
        {
            try
            {
                var client = await _clientService.GetClientByEmailAsync(email);
                if (client == null) return NotFound($"Client with {email} not found.");
                return Ok(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientByEmail: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("profile")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetClientProfile()
        {
            try
            {
                var client = await _clientService.GetClientAsync();
                if (client == null) return NotFound("Client profile not found.");
                return Ok(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientProfile: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = await _clientService.UpdateClientAsync(updateDto);
                if (!result) return BadRequest("Failed to update client.");

                return Ok("Client updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateClient: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> DeleteClient()
        {
            try
            {
                var result = await _clientService.DeleteClientAsync();
                if (!result) return BadRequest("Failed to delete client.");

                return Ok("Client deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteClient: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClientByEmail(string email)
        {
            try
            {
                var result = await _clientService.DeleteClientAsync(email);
                if (!result) return BadRequest("Failed to delete client.");

                return Ok("Client deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteClientByEmail: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
