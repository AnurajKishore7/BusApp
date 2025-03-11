using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
    public interface ITripsService
    {
        Task<IEnumerable<TripResponseDto>> GetAllTripsAsync();
        Task<TripResponseDto?> GetTripByIdAsync(int id);
        Task<IEnumerable<TripResponseDto>> GetTripsByRouteAsync(int busRouteId);
        Task<IEnumerable<TripResponseDto>> GetTripsBySourceAndDestinationAsync(string source, string destination);
        Task<TripResponseDto?> AddTripAsync(TripDto dto);
        Task<TripResponseDto?> UpdateTripAsync(int id, TripDto dto);
        Task<bool> DeleteTripAsync(int id);
        Task<TripDetailsResponseDto?> GetTripDetailsAsync(int id, DateTime journeyDate);
    }
}
