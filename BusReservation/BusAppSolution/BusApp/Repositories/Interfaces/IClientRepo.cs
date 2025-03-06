using BusApp.DTOs;
using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface IClientRepo
    {
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client?> GetClientByEmailAsync(string email);
        Task<bool> UpdateClientAsync(int clientId, UpdateClientDto updateDto);
        Task<bool> DeleteClientAsync(string email);
    }

}
