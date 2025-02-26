using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface IBookingRepo
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetAllWithTripDetailsAsync();
        Task<Booking?> GetByIdAsync(int bookingId);
        Task<Booking?> GetByIdWithTripDetailsAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByClientIdAsync(int clientId);
        Task<IEnumerable<Booking>> GetBookingsByClientIdWithTripDetailsAsync(int clientId);
        Task<Booking> AddBookingAsync(Booking booking);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<int> GetBookedSeatsByTripIdAsync(int tripId, DateTime journeyDate);

    }

}
