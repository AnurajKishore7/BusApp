using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface ITicketPassengerRepo
    {
        Task<IEnumerable<TicketPassenger>> GetAllAsync();
        Task<TicketPassenger?> GetByIdAsync(int ticketPassengerId);
        Task<IEnumerable<TicketPassenger>> GetByBookingIdAsync(int bookingId);
        Task<IEnumerable<TicketPassenger>> AddPassengersAsync(IEnumerable<TicketPassenger> passengers);
    }

}
