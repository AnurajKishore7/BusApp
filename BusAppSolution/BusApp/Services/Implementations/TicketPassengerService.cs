using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Implementations;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;

namespace BusApp.Services.Implementations
{
    public class TicketPassengerService : ITicketPassengerService
    {
        private readonly ITicketPassengerRepo _repo;

        public TicketPassengerService(ITicketPassengerRepo ticketPassengerRepo)
        {
            _repo = ticketPassengerRepo;
        }

        public async Task<IEnumerable<TicketPassengerResponseDto>> GetAllAsync()
        {
            try
            {
                var passengers = await _repo.GetAllAsync();

                return passengers.Select(p => new TicketPassengerResponseDto
                {
                    Id = p.Id,
                    BookingId = p.BookingId,
                    PassengerName = p.Name,
                    Contact = p.Contact,
                    IsHandicapped = p.IsHandicapped
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync (Service): {ex.Message}");
                return new List<TicketPassengerResponseDto>();
            }
        }

        public async Task<TicketPassengerResponseDto?> GetByIdAsync(int passengerId)
        {
            try
            {
                var passenger = await _repo.GetByIdAsync(passengerId);
                if (passenger == null)
                    return null;

                return new TicketPassengerResponseDto
                {
                    Id = passenger.Id,
                    BookingId = passenger.BookingId,
                    PassengerName = passenger.Name,
                    Contact = passenger.Contact,
                    IsHandicapped = passenger.IsHandicapped
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync (Service) for PassengerId {passengerId}: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<TicketPassengerResponseDto>> GetByBookingIdAsync(int bookingId)
        {
            try
            {
                var passengers = await _repo.GetByBookingIdAsync(bookingId);

                return passengers.Select(p => new TicketPassengerResponseDto
                {
                    Id = p.Id,
                    BookingId = p.BookingId,
                    PassengerName = p.Name,
                    Contact = p.Contact,
                    IsHandicapped = p.IsHandicapped
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByBookingIdAsync (Service) for BookingId {bookingId}: {ex.Message}");
                return new List<TicketPassengerResponseDto>();
            }
        }

        public async Task<IEnumerable<TicketPassengerResponseDto>> AddPassengersAsync(int bookingId, IEnumerable<TicketPassengerDto> passengerDtos)
        {
            try
            {
                // Convert DTOs to TicketPassenger entities
                var passengers = passengerDtos.Select(dto => new TicketPassenger
                {
                    BookingId = bookingId,
                    Name = dto.PassengerName,
                    Contact = dto.Contact,
                    IsHandicapped = dto.IsHandicapped
                }).ToList();

                var addedPassengers = await _repo.AddPassengersAsync(passengers);

                // Convert saved entities to response DTOs
                return addedPassengers.Select(p => new TicketPassengerResponseDto
                {
                    Id = p.Id,
                    BookingId = p.BookingId,
                    PassengerName = p.Name,
                    Contact = p.Contact,
                    IsHandicapped = p.IsHandicapped
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddPassengersAsync (Service) for BookingId {bookingId}: {ex.Message}");
                return new List<TicketPassengerResponseDto>();
            }
        }
    }

}
