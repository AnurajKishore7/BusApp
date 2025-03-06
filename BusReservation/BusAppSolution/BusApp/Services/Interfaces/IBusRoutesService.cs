using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
    public interface IBusRoutesService
    {
        Task<IEnumerable<BusRouteResponseDto>> GetAllBusRoutesAsync();
        Task<BusRouteResponseDto?> GetBusRouteByIdAsync(int id);
        Task<BusRouteResponseDto?> SearchBusRouteAsync(string? source, string? destination);
        Task<BusRouteResponseDto?> AddBusRouteAsync(BusRouteDto newBusRouteDto);
        Task<bool> UpdateBusRouteAsync(int id, BusRouteDto updatedBusRouteDto);
        Task<bool> DeleteBusRouteAsync(int id);
    }

}
