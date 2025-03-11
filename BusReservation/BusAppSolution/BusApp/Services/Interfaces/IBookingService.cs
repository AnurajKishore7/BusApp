using BusApp.DTOs;
using BusApp.Models;

namespace BusApp.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponseDto>> GetAllAsync();
        Task<BookingResponseDto?> GetByIdAsync(int bookingId);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByClientIdAsync(int clientId);
        Task<BookingResponseDto> AddBookingAsync(BookingDto bookingDto, string userEmail);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<int> GetAvailableSeatsAsync(int tripId, DateTime journeyDate);
        Task<IEnumerable<TripSearchDetailsDto>> SearchTripsAsync(string source, string destination, DateTime journeyDate);
        Task<TripSearchDetailsDto?> GetTripDetailsAsync(int tripId, DateTime journeyDate);
        Task<SeatLayoutDto> GetSeatLayoutAsync(int tripId, DateTime journeyDate);
    }
}
