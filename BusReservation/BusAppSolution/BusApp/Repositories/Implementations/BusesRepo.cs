using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class BusesRepo : IBusesRepo
    {
        private readonly AppDbContext _context;

        public BusesRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bus>> GetAllBusesAsync()
        {
            try
            {
                return await _context.Buses.Where(t => !t.IsDeleted).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllBusesAsync (Repo): {ex.Message}");
                return new List<Bus>();
            }

        }

        public async Task<Bus?> GetBusByIdAsync(int busId)
        {
            try
            {
                return await _context.Buses
                    .FirstOrDefaultAsync(b => b.Id == busId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBusByIdAsync (Repo): {ex.Message}");
                return null;
            }

        }

        public async Task<IEnumerable<Bus>> GetMyBusesAsync(int operatorId)
        {
            try
            {
                return await _context.Buses
                    .Where(b => b.OperatorId == operatorId && !b.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyBusesAsync (Repo): {ex.Message}");
                return new List<Bus>(); 
            }

        }

        public async Task<bool> AddBusAsync(Bus bus)
        {
            try
            {
                await _context.Buses.AddAsync(bus);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBusAsync (Repo): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBusAsync(int busId)
        {
            try
            {
                var bus = await _context.Buses.FirstOrDefaultAsync(b => b.Id == busId && !b.IsDeleted);
                if (bus == null) return false;

                // Soft delete
                bus.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteBusAsync (Repo): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateBusAsync(int busId, UpdateBusDto updateDto)
        {
            try
            {
                var bus = await _context.Buses.FirstOrDefaultAsync(b => b.Id == busId);
                if (bus == null) return false;

                // Updating bus details
                bus.BusNo = updateDto.BusNo;
                bus.BusType = updateDto.BusType;
                bus.TotalSeats = updateDto.TotalSeats;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBusAsync (Repo): {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetTotalSeatsByBusIdAsync(int busId)
        {
            try
            {
                var bus = await _context.Buses.FindAsync(busId);
                if (bus == null)
                {
                    Console.WriteLine($"Bus not found for BusId: {busId}");
                    return 0;
                }

                return bus.TotalSeats;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTotalSeatsByBusIdAsync for BusId {busId}: {ex.Message}");
                return 0;
            }
        }

        

    }
}
