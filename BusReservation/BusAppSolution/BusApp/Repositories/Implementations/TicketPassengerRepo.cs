using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class TicketPassengerRepo : ITicketPassengerRepo
    {
        private readonly AppDbContext _context;

        public TicketPassengerRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketPassenger>> GetAllAsync()
        {
            try
            {
                return await _context.TicketPassengers.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync (Repo): {ex.Message}");
                return new List<TicketPassenger>();
            }
        }

        public async Task<TicketPassenger?> GetByIdAsync(int ticketPassengerId)
        {
            try
            {
                return await _context.TicketPassengers.FirstOrDefaultAsync(tp => tp.Id == ticketPassengerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<TicketPassenger>> GetByBookingIdAsync(int bookingId)
        {
            try
            {
                return await _context.TicketPassengers
                    .Where(tp => tp.BookingId == bookingId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByBookingIdAsync (Repo): {ex.Message}");
                return new List<TicketPassenger>();
            }
        }

        public async Task<IEnumerable<TicketPassenger>> AddPassengersAsync(IEnumerable<TicketPassenger> passengers)
        {
            try
            {
                await _context.TicketPassengers.AddRangeAsync(passengers);
                int savedCount = await _context.SaveChangesAsync();
                Console.WriteLine($"Successfully saved {savedCount} ticket passengers to the database.");
                return passengers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddPassengersAsync (Repo): {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return new List<TicketPassenger>();
            }
        }
    }
}
