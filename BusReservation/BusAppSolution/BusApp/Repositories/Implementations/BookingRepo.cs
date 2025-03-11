using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class BookingRepo : IBookingRepo
    {
        private readonly AppDbContext _context;

        public BookingRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            try
            {
                return await _context.Bookings.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync (Repo): {ex.Message}");
                return new List<Booking>();
            }
        }

        public async Task<IEnumerable<Booking>> GetAllWithTripDetailsAsync()
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                    .Include(b => b.Client) 
                    .Include(b => b.TicketPassengers)
                    .Include(b => b.Payment)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllWithTripDetailsAsync (Repo): {ex.Message}");
                return new List<Booking>();
            }
        }


        public async Task<Booking?> GetByIdAsync(int bookingId)
        {
            try
            {
                return await _context.Bookings.FindAsync(bookingId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<Booking?> GetByIdWithTripDetailsAsync(int bookingId)
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Trip)   
                    .ThenInclude(t => t.BusRoute) 
                    .Include(b => b.Client) 
                    .Include(b => b.TicketPassengers)
                    .Include(b => b.Payment)
                    .FirstOrDefaultAsync(b => b.Id == bookingId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdWithTripDetailsAsync (Repo): {ex.Message}");
                return null;
            }
        }


        public async Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(int clientId)
        {
            try
            {
                return await _context.Bookings
                    .Where(b => b.ClientId == clientId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookingsByClientIdAsync (Repo): {ex.Message}");
                return new List<Booking>();
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByClientIdWithTripDetailsAsync(int clientId)
        {
            try
            {
                return await _context.Bookings
                    .Where(b => b.ClientId == clientId)
                    .Include(b => b.Trip)
                    .ThenInclude(t => t.BusRoute)
                    .Include(b => b.Client)
                    .Include(b => b.TicketPassengers)
                    .Include(b => b.Payment)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookingsByClientIdWithTripDetailsAsync (Repo): {ex.Message}");
                return new List<Booking>();
            }
        }


        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            try
            {
                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();
                return booking;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBookingAsync (Repo): {ex.Message}");
                return null!;
            }
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                {
                    Console.WriteLine($"Booking with ID {bookingId} not found.");
                    return false;
                }

                booking.Status = "Cancelled";
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CancelBookingAsync (Repo): {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetBookedSeatsByTripIdAsync(int tripId, DateTime journeyDate)
        {
            try
            {
                return await _context.Bookings
                    .Where(b => b.TripId == tripId && b.JourneyDate == journeyDate && b.Status == "Confirmed")
                    .SumAsync(b => b.TicketCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookedSeatsByTripIdAsync for TripId {tripId} on {journeyDate}: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<string>> GetBookedSeatNumbersAsync(int tripId, DateTime journeyDate)
        {
            try
            {
                // Get all bookings for the trip and date with status "Confirmed"
                var bookings = await _context.Bookings
                    .Where(b => b.TripId == tripId && b.JourneyDate == journeyDate && b.Status == "Confirmed")
                    .Select(b => b.Id)
                    .ToListAsync();

                // Get all seat numbers from TicketPassengers for these bookings
                var bookedSeatNumbers = await _context.TicketPassengers
                    .Where(tp => bookings.Contains(tp.BookingId))
                    .Select(tp => tp.SeatNumber)
                    .ToListAsync();

                return bookedSeatNumbers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookedSeatNumbersAsync for TripId {tripId} on {journeyDate}: {ex.Message}");
                return new List<string>();
            }
        }
    }
}
