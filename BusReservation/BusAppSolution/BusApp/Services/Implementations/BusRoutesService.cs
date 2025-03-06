using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;

namespace BusApp.Services.Implementations
{
    public class BusRoutesService : IBusRoutesService
    {
        private readonly IBusRoutesRepo _repo;

        public BusRoutesService(IBusRoutesRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<BusRouteResponseDto>> GetAllBusRoutesAsync()
        {
            try
            {
                var busRoutes = await _repo.GetAllBusRoutesAsync();
                return busRoutes.Select(route => new BusRouteResponseDto
                {
                    Id = route.Id,
                    Source = route.Source,
                    Destination = route.Destination,
                    EstimatedDuration = route.EstimatedDuration,
                    Distance = route.Distance
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllBusRoutesAsync (Service): {ex.Message}");
                return new List<BusRouteResponseDto>();
            }
        }

        public async Task<BusRouteResponseDto?> GetBusRouteByIdAsync(int id)
        {
            try
            {
                var busRoute = await _repo.GetBusRouteByIdAsync(id);
                if (busRoute == null || busRoute.IsDeleted)
                    throw new KeyNotFoundException("Bus route not found.");

                return new BusRouteResponseDto
                {
                    Id = busRoute.Id,
                    Source = busRoute.Source,
                    Destination = busRoute.Destination,
                    EstimatedDuration = busRoute.EstimatedDuration,
                    Distance = busRoute.Distance
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBusRouteByIdAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<BusRouteResponseDto?> SearchBusRouteAsync(string? source, string? destination)
        {
            try
            {
                // Input validation
                source = source?.Trim().ToLower();
                destination = destination?.Trim().ToLower();

                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
                    throw new ArgumentException("Source and destination cannot be empty.");

                var busRoute = await _repo.SearchBusRouteAsync(source, destination);

                if (busRoute == null || busRoute.IsDeleted)
                    throw new KeyNotFoundException("Bus route not found.");

                return new BusRouteResponseDto
                {
                    Id = busRoute.Id,
                    Source = busRoute.Source,
                    Destination = busRoute.Destination,
                    EstimatedDuration = busRoute.EstimatedDuration,
                    Distance = busRoute.Distance
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchBusRouteAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<BusRouteResponseDto?> AddBusRouteAsync(BusRouteDto newBusRouteDto)
        {
            try
            {
                if (newBusRouteDto.Source == newBusRouteDto.Destination)
                    throw new ArgumentException("Source and destination cannot be the same.");

                var newBusRoute = new BusRoute
                {
                    Source = newBusRouteDto.Source,
                    Destination = newBusRouteDto.Destination,
                    EstimatedDuration = newBusRouteDto.EstimatedDuration,
                    Distance = newBusRouteDto.Distance,
                    IsDeleted = false
                };

                var createdBusRoute = await _repo.AddBusRouteAsync(newBusRoute);
                if (createdBusRoute == null)
                    throw new Exception("Failed to add the bus route.");

                return new BusRouteResponseDto
                {
                    Id = createdBusRoute.Id,
                    Source = createdBusRoute.Source,
                    Destination = createdBusRoute.Destination,
                    EstimatedDuration = createdBusRoute.EstimatedDuration,
                    Distance = createdBusRoute.Distance
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBusRouteAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateBusRouteAsync(int id, BusRouteDto updatedBusRouteDto)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                    throw new ArgumentException("Invalid bus route ID.");

                if (updatedBusRouteDto.Source == updatedBusRouteDto.Destination)
                    throw new ArgumentException("Source and destination cannot be the same.");

                var existingRoute = await _repo.GetBusRouteByIdAsync(id);
                if (existingRoute == null || existingRoute.IsDeleted)
                    throw new KeyNotFoundException("Bus route not found.");

                // Update properties
                existingRoute.Source = updatedBusRouteDto.Source;
                existingRoute.Destination = updatedBusRouteDto.Destination;
                existingRoute.EstimatedDuration = updatedBusRouteDto.EstimatedDuration;
                existingRoute.Distance = updatedBusRouteDto.Distance;

                // Save changes
                var result = await _repo.UpdateBusRouteAsync(existingRoute);

                if (!result)
                    throw new Exception("Failed to update the bus route.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBusRouteAsync (Service): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteBusRouteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid bus route ID.");

                return await _repo.DeleteBusRouteAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteBusRouteAsync (Service): {ex.Message}");
                throw; 
            }
        }
    }
}
