using BusApp.DTOs;
using BusApp.Repositories.Interfaces;
using System.Security.Claims;
using BusApp.Services.Interfaces;

namespace BusApp.Services.Implementations
{
    public class OperatorService : IOperatorService
    {
        private readonly IOperatorRepo _operatorRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OperatorService(IOperatorRepo operatorRepo, IHttpContextAccessor httpContextAccessor)
        {
            _operatorRepo = operatorRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<OperatorDto>> GetAllOperatorsAsync()
        {
            try
            {
                var operators = await _operatorRepo.GetAllOperatorsAsync();
                return operators.Select(c => new OperatorDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Contact = c.Contact
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllOperators (Service): {ex.Message}");
                return Enumerable.Empty<OperatorDto>();
            }
        }

        public async Task<OperatorDto?> GetOperatorByIdAsync(int id)
        {
            try
            {
                var client = await _operatorRepo.GetOperatorByIdAsync(id);
                if (client == null) return null;

                return new OperatorDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    Contact = client.Contact
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientById (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<OperatorDto?> GetOperatorByEmailAsync(string email)
        {
            try
            {
                var transportOperator = await _operatorRepo.GetOperatorByEmailAsync(email);
                if (transportOperator == null) return null;

                return new OperatorDto
                {
                    Id = transportOperator.Id,
                    Name = transportOperator.Name,
                    Email = transportOperator.Email,
                    Contact = transportOperator.Contact
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOperatorByEmail (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<OperatorDto?> GetOperatorAsync()
        {
            try
            {
                var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
                if (email == null) return null;

                var transportOperator = await _operatorRepo.GetOperatorByEmailAsync(email);
                if (transportOperator == null) return null;

                return new OperatorDto
                {
                    Id = transportOperator.Id,
                    Name = transportOperator.Name,
                    Email = transportOperator.Email,
                    Contact = transportOperator.Contact
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOperator (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateOperatorAsync(UpdateOperatorDto updateDto)
        {
            try
            {
                var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Error: Email not found in JWT claims.");
                    return false;
                }

                var transportOperator = await _operatorRepo.GetOperatorByEmailAsync(email);
                if (transportOperator == null) return false;

                return await _operatorRepo.UpdateOperatorAsync(transportOperator.Id, updateDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateOperator (Service): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOperatorAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Error: Email is null or empty.");
                    return false;
                }

                return await _operatorRepo.DeleteOperatorAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteOperatorByEmail (Service): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOperatorAsync()
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

                // Call repository method to delete the transport operator
                return await _operatorRepo.DeleteOperatorAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteOperator (Service): {ex.Message}");
                return false;
            }
        }
    }
}
