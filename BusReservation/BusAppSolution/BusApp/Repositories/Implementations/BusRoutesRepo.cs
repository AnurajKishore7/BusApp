using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class BusRoutesRepo : IBusRoutesRepo
    {
        private readonly AppDbContext _context;

        public BusRoutesRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusRoute>> GetAllBusRoutesAsync()
        {
            try
            {
                return await _context.BusRoutes.Where(t => !t.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllBusRoutesAsync (Repo): {ex.Message}");
                return new List<BusRoute>(); 
            }
        }

        public async Task<BusRoute?> GetBusRouteByIdAsync(int id)
        {
            try
            {
                return await _context.BusRoutes.FirstOrDefaultAsync(br => br.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBusRouteByIdAsync (Repo): {ex.Message}");
                return null; 
            }
        }

        public async Task<BusRoute?> SearchBusRouteAsync(string source, string destination)
        {
            try
            {
                return await _context.BusRoutes
                    .FirstOrDefaultAsync(br => br.Source == source && br.Destination == destination);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchBusRouteAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<BusRoute?> AddBusRouteAsync(BusRoute newBusRoute)
        {
            try
            {
                var createdEntity = await _context.BusRoutes.AddAsync(newBusRoute);
                await _context.SaveChangesAsync();
                return createdEntity.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBusRouteAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateBusRouteAsync(BusRoute updatedBusRoute)
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBusRouteAsync (Repo): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBusRouteAsync(int id)
        {
            try
            {
                var busRoute = await _context.BusRoutes.FindAsync(id);
                if (busRoute == null || busRoute.IsDeleted) return false;

                busRoute.IsDeleted = true;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteBusRouteAsync (Repo): {ex.Message}");
                return false;
            }
        }
    }
}
