using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface IBusRoutesRepo
    {
        Task<IEnumerable<BusRoute>> GetAllBusRoutesAsync();
        Task<BusRoute?> GetBusRouteByIdAsync(int id);
        Task<BusRoute?> SearchBusRouteAsync(string source, string destination);
        Task<BusRoute?> AddBusRouteAsync(BusRoute newBusRoute);
        Task<bool> UpdateBusRouteAsync(BusRoute updatedBusRoute);
        Task<bool> DeleteBusRouteAsync(int id);
    }

}
