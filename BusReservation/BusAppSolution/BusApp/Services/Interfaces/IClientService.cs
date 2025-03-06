using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
        Task<ClientDto?> GetClientByIdAsync(int id);
        Task<ClientDto?> GetClientByEmailAsync(string email);
        Task<ClientDto?> GetClientAsync();
        Task<bool> UpdateClientAsync(UpdateClientDto updateDto);
        Task<bool> DeleteClientAsync();
        Task<bool> DeleteClientAsync(string email);
    }
}
