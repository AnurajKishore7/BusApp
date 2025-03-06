using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface ITripsRepo
    {
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip?> GetTripByIdAsync(int id);
        Task<IEnumerable<Trip>> GetTripsByRouteAsync(int busRouteId);
        Task<IEnumerable<Trip>> GetTripsBySourceAndDestinationAsync(string source, string destination);
        Task<Trip?> AddTripAsync(Trip trip);
        Task<Trip?> UpdateTripAsync(Trip trip);
        Task<bool> DeleteTripAsync(int id);
    }

}
