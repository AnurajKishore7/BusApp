using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
    public interface ITicketPassengerService
    {
        Task<IEnumerable<TicketPassengerResponseDto>> GetAllAsync();
        Task<TicketPassengerResponseDto?> GetByIdAsync(int ticketPassengerId);
        Task<IEnumerable<TicketPassengerResponseDto>> GetByBookingIdAsync(int bookingId);
        Task<IEnumerable<TicketPassengerResponseDto>> AddPassengersAsync(int bookingId, IEnumerable<TicketPassengerDto> passengers);
    }

}
