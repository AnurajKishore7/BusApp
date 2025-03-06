using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class TripsRepo : ITripsRepo
    {
        private readonly AppDbContext _context;

        public TripsRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            try
            {
                return await _context.Trips.Where(t => !t.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllTripsAsync (Repo): {ex.Message}");
                return new List<Trip>();
            }
        }

        public async Task<Trip?> GetTripByIdAsync(int id)
        {
            try
            {
                return await _context.Trips.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTripByIdAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Trip>> GetTripsByRouteAsync(int busRouteId)
        {
            try
            {
                return await _context.Trips
                    .Where(t => t.BusRouteId == busRouteId && !t.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTripsByRouteAsync (Repo): {ex.Message}");
                return new List<Trip>();
            }
        }

        public async Task<IEnumerable<Trip>> GetTripsBySourceAndDestinationAsync(string source, string destination)
        {
            try
            {
                return await _context.Trips
                    .Include(t => t.BusRoute)
                    .Where(t => t.BusRoute != null &&
                                t.BusRoute.Source == source &&
                                t.BusRoute.Destination == destination &&
                                !t.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTripsBySourceAndDestinationAsync (Repo): {ex.Message}");
                return new List<Trip>();
            }
        }

        public async Task<Trip?> AddTripAsync(Trip trip)
        {
            try
            {
                await _context.Trips.AddAsync(trip);
                await _context.SaveChangesAsync();
                return trip;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddTripAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<Trip?> UpdateTripAsync(Trip trip)
        {
            try
            {
                await _context.SaveChangesAsync();
                return trip;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateTripAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            try
            {
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                    return false;

                trip.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteTripAsync (Repo): {ex.Message}");
                return false;
            }
        }


    }
}
