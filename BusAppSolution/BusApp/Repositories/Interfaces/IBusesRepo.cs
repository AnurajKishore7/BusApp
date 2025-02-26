using BusApp.DTOs;
using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface IBusesRepo
    {
        Task<IEnumerable<Bus>> GetAllBusesAsync();
        Task<IEnumerable<Bus>> GetMyBusesAsync(int operatorId);
        Task<Bus?> GetBusByIdAsync(int busId);
        Task<bool> AddBusAsync(Bus bus);
        Task<bool> UpdateBusAsync(int busId, UpdateBusDto updateDto);
        Task<bool> DeleteBusAsync(int busId);
        Task<int> GetTotalSeatsByBusIdAsync(int busId);
    }
}
