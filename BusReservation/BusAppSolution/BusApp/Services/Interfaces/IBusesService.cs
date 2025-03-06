using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
    public interface IBusesService
    {
        Task<IEnumerable<BusDto>> GetAllBusesAsync();
        Task<IEnumerable<BusDto>> GetMyBusesAsync(string email);
        Task<BusDto?> GetBusByIdAsync(int id, string role, string email);
        Task<bool> AddBusAsync(NewBusDto newBusDto, string email);
        Task<bool> UpdateBusAsync(string email, int busId, UpdateBusDto updateBusDto);
        Task<bool> DeleteBusAsync(string email, int busId);
    }
}
