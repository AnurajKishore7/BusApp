using BusApp.DTOs;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;
using System.Security.Claims;

namespace BusApp.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IClientRepo _clientRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientService(IClientRepo clientRepo, IHttpContextAccessor httpContextAccessor)
        {
            _clientRepo = clientRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            try
            {
                var clients = await _clientRepo.GetAllClientsAsync();
                return clients.Select(c => new ClientDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    DOB = c.DOB,
                    Gender = c.Gender,
                    Contact = c.Contact,
                    IsHandicapped = c.IsHandicapped
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllClients (Service): {ex.Message}");
                return Enumerable.Empty<ClientDto>();
            }
        }

        public async Task<ClientDto?> GetClientByIdAsync(int id)
        {
            try
            {
                var client = await _clientRepo.GetClientByIdAsync(id);
                if (client == null) return null;

                return new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    DOB = client.DOB,
                    Gender = client.Gender,
                    Contact = client.Contact,
                    IsHandicapped = client.IsHandicapped
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientById (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<ClientDto?> GetClientByEmailAsync(string email)
        {
            try
            {
                var client = await _clientRepo.GetClientByEmailAsync(email);
                if (client == null) return null;

                return new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    DOB = client.DOB,
                    Gender = client.Gender,
                    Contact = client.Contact,
                    IsHandicapped = client.IsHandicapped
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientByEmail (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<ClientDto?> GetClientAsync()
        {
            try
            {
                var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
                if (email == null) return null;

                var client = await _clientRepo.GetClientByEmailAsync(email);
                if (client == null) return null;

                return new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    DOB = client.DOB,
                    Gender = client.Gender,
                    Contact = client.Contact,
                    IsHandicapped = client.IsHandicapped
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClient (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateClientAsync(UpdateClientDto updateDto)
        {
            try
            {
                var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Error: Email not found in JWT claims.");
                    return false;
                }

                var client = await _clientRepo.GetClientByEmailAsync(email);
                if (client == null) return false;

                return await _clientRepo.UpdateClientAsync(client.Id, updateDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateClient (Service): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteClientAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Error: Email is null or empty.");
                    return false;
                }

                return await _clientRepo.DeleteClientAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteClientByEmail (Service): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteClientAsync()
        {
            try
            {
                // Extract email from JWT token
                var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Error: Email not found in JWT claims.");
                    return false;
                }

                // Call repository method to delete the client
                return await _clientRepo.DeleteClientAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteClient (Service): {ex.Message}");
                return false;
            }
        }
    }
}
