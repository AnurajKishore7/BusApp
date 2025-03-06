using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;

namespace BusApp.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly ITicketPassengerRepo _ticketPassengerRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly ITripsRepo _tripRepo;
        private readonly IClientRepo _clientRepo;
        private readonly IBusesRepo _busRepo;
        private readonly IBusRoutesRepo _busRouteRepo;
        private readonly IOperatorRepo _operatorRepo;

        public BookingService(
            IBookingRepo bookingRepo,
            ITicketPassengerRepo ticketPassengerRepo,
            IPaymentRepo paymentRepo,
            ITripsRepo tripRepo,
            IClientRepo clientRepo,
            IBusesRepo busRepo,
            IBusRoutesRepo busRouteRepo,
            IOperatorRepo operatorRepo)
        {
            _bookingRepo = bookingRepo;
            _ticketPassengerRepo = ticketPassengerRepo;
            _paymentRepo = paymentRepo;
            _tripRepo = tripRepo;
            _clientRepo = clientRepo;
            _busRepo = busRepo;
            _busRouteRepo = busRouteRepo;
            _operatorRepo = operatorRepo;
        }


        public async Task<IEnumerable<BookingResponseDto>> GetAllAsync()
        {
            try
            {
                var bookings = await _bookingRepo.GetAllWithTripDetailsAsync();

                var bookingDtos = bookings.Select(booking => new BookingResponseDto
                {
                    Id = booking.Id,
                    ClientId = booking.ClientId,
                    TripId = booking.TripId,
                    JourneyDate = booking.JourneyDate,
                    TicketCount = booking.TicketCount,
                    Status = booking.Status,
                    ClientName = booking.Client?.Name ?? "Unknown",
                    Source = booking.Trip.BusRoute.Source,
                    Destination = booking.Trip.BusRoute.Destination,
                    TotalAmount = booking.Payment.TotalAmount,
                    PaymentMethod = booking.Payment.PaymentMethod,
                    PaymentStatus = booking.Payment.Status,
                    TicketPassengers = booking.TicketPassengers.Select(p => new TicketPassengerResponseDto
                    {
                        Id = p.Id,
                        BookingId = p.BookingId,
                        PassengerName = p.Name,
                        Contact = p.Contact,
                        IsHandicapped = p.IsHandicapped
                    }).ToList()
                }).ToList();

                return bookingDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync (Service): {ex.Message}");
                return new List<BookingResponseDto>();
            }
        }

        public async Task<BookingResponseDto?> GetByIdAsync(int bookingId)
        {
            try
            {
                var booking = await _bookingRepo.GetByIdWithTripDetailsAsync(bookingId);
                if (booking == null) return null;

                return new BookingResponseDto
                {
                    Id = booking.Id,
                    ClientId = booking.ClientId,
                    TripId = booking.TripId,
                    JourneyDate = booking.JourneyDate,
                    TicketCount = booking.TicketCount,
                    Status = booking.Status,
                    ClientName = booking.Client?.Name ?? "Unknown",
                    Source = booking.Trip.BusRoute.Source,
                    Destination = booking.Trip.BusRoute.Destination,
                    TotalAmount = booking.Payment.TotalAmount,
                    PaymentMethod = booking.Payment.PaymentMethod,
                    PaymentStatus = booking.Payment.Status,
                    TicketPassengers = booking.TicketPassengers.Select(p => new TicketPassengerResponseDto
                    {
                        Id = p.Id,
                        BookingId = p.BookingId,
                        PassengerName = p.Name,
                        Contact = p.Contact,
                        IsHandicapped = p.IsHandicapped
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByClientIdAsync(int clientId)
        {
            try
            {
                var bookings = await _bookingRepo.GetBookingsByClientIdWithTripDetailsAsync(clientId);
                return bookings.Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    ClientId = b.ClientId,
                    TripId = b.TripId,
                    JourneyDate = b.JourneyDate,
                    TicketCount = b.TicketCount,
                    Status = b.Status,
                    TicketPassengers = b.TicketPassengers?.Select(tp => new TicketPassengerResponseDto
                    {
                        Id = tp.Id,
                        BookingId = tp.BookingId,
                        PassengerName = tp.Name,
                        Contact = tp.Contact,
                        IsHandicapped = tp.IsHandicapped
                    }).ToList() ?? new List<TicketPassengerResponseDto>(),
                    ClientName = b.Client?.Name ?? "",
                    Source = b.Trip?.BusRoute?.Source ?? "", 
                    Destination = b.Trip?.BusRoute?.Destination ?? "",
                    TotalAmount = b.Payment?.TotalAmount ?? 0,
                    PaymentMethod = b.Payment?.PaymentMethod ?? "N/A",
                    PaymentStatus = b.Payment?.Status ?? "Pending"
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookingsByClientIdAsync: {ex.Message}");
                return new List<BookingResponseDto>();
            }
        }

        public async Task<BookingResponseDto> AddBookingAsync(BookingDto bookingDto, string userEmail)
        {
            try
            {
                // 1. Validate trip existence
                var trip = await _tripRepo.GetTripByIdAsync(bookingDto.TripId);
                if (trip == null)
                {
                    throw new Exception("Invalid trip ID.");
                }

                var busRoute = await _busRouteRepo.GetBusRouteByIdAsync(trip.BusRouteId);
                if (busRoute == null)
                {
                    throw new Exception("Invalid busRoute ID.");
                }

                // 2. Check seat availability
                int totalSeats = await _busRepo.GetTotalSeatsByBusIdAsync(trip.BusId);
                int bookedSeats = await _bookingRepo.GetBookedSeatsByTripIdAsync(bookingDto.TripId, bookingDto.JourneyDate);

                if (bookedSeats + bookingDto.TicketCount > totalSeats)
                {
                    throw new Exception("Not enough seats available.");
                }

                // 3. Fetch Client ID from User Email
                var client = await _clientRepo.GetClientByEmailAsync(userEmail);
                if (client == null)
                {
                    throw new Exception("Client not found.");
                }

                // 4. Create a Booking entity with "Pending" status
                var newBooking = new Booking
                {
                    ClientId = client.Id,
                    TripId = bookingDto.TripId,
                    JourneyDate = bookingDto.JourneyDate,
                    TicketCount = bookingDto.TicketCount,
                    Status = "Pending" // Set to "Pending" until payment is confirmed
                };

                var addedBooking = await _bookingRepo.AddBookingAsync(newBooking);

                // 5. Add TicketPassengers
                var passengers = bookingDto.TicketPassengers.Select(p => new TicketPassenger
                {
                    BookingId = addedBooking.Id,
                    Name = p.PassengerName,
                    Contact = p.Contact,
                    IsHandicapped = p.IsHandicapped
                }).ToList();

                await _ticketPassengerRepo.AddPassengersAsync(passengers);

                // 6. Calculate Total Amount
                decimal totalAmount = trip.Price * bookingDto.TicketCount;

                // 7. Create a Payment entry with "Pending" status
                var payment = new Payment
                {
                    BookingId = addedBooking.Id,
                    TotalAmount = totalAmount,
                    PaymentMethod = "Online",
                    Status = "Pending" // Will be updated when payment is confirmed
                };

                var addedPayment = await _paymentRepo.AddPaymentAsync(payment);

                // 8. Prepare Response
                var response = new BookingResponseDto
                {
                    Id = addedBooking.Id,
                    ClientId = client.Id,
                    TripId = bookingDto.TripId,
                    JourneyDate = bookingDto.JourneyDate,
                    TicketCount = bookingDto.TicketCount,
                    Status = addedBooking.Status, // "Pending"
                    ClientName = client.Name,
                    Source = busRoute.Source,
                    Destination = busRoute.Destination,
                    TotalAmount = totalAmount,
                    PaymentMethod = payment.PaymentMethod,
                    PaymentStatus = payment.Status, // "Pending"
                    TicketPassengers = passengers.Select(p => new TicketPassengerResponseDto
                    {
                        Id = p.Id,
                        BookingId = p.BookingId,
                        PassengerName = p.Name,
                        Contact = p.Contact,
                        IsHandicapped = p.IsHandicapped
                    }).ToList()
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBookingAsync (Service): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            try
            {
                return await _bookingRepo.CancelBookingAsync(bookingId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CancelBookingAsync (Service): {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetAvailableSeatsAsync(int tripId, DateTime journeyDate)
        {
            try
            {
                // Get the trip details
                var trip = await _tripRepo.GetTripByIdAsync(tripId);
                if (trip == null)
                {
                    throw new Exception("Trip not found.");
                }

                // Get total seats for the bus
                int totalSeats = await _busRepo.GetTotalSeatsByBusIdAsync(trip.BusId);

                // Get booked seats for the given trip and date
                int bookedSeats = await _bookingRepo.GetBookedSeatsByTripIdAsync(tripId, journeyDate);

                // Calculate available seats
                int availableSeats = totalSeats - bookedSeats;
                return availableSeats >= 0 ? availableSeats : 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableSeatsAsync for TripId {tripId} on {journeyDate}: {ex.Message}");
                return 0;
            }
        }

        public async Task<IEnumerable<TripSearchDetailsDto>> SearchTripsAsync(string source, string destination, DateTime journeyDate)
        {
            try
            {
                // Get trips by source and destination
                var trips = await _tripRepo.GetTripsBySourceAndDestinationAsync(source, destination);
                var tripDetailsList = new List<TripSearchDetailsDto>();

                foreach (var trip in trips)
                {
                    // Get bus details
                    var bus = await _busRepo.GetBusByIdAsync(trip.BusId);
                    if (bus == null) continue;

                    // Get operator name
                    var operatorEntity = await _operatorRepo.GetOperatorByIdAsync(bus.OperatorId);
                    if (operatorEntity == null) continue;

                    // Get available seats
                    int availableSeats = await GetAvailableSeatsAsync(trip.Id, journeyDate);

                    tripDetailsList.Add(new TripSearchDetailsDto
                    {
                        TripId = trip.Id,
                        OperatorName = operatorEntity.Name,
                        BusNo = bus.BusNo,
                        Departure = trip.DepartureTime,
                        Arrival = trip.ArrivalTime,
                        PricePerSeat = trip.Price,
                        SeatsAvailable = availableSeats,
                        TotalSeats = bus.TotalSeats,
                        BusType = bus.BusType
                    });
                }

                return tripDetailsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SearchTripsAsync (Service): {ex.Message}");
                return new List<TripSearchDetailsDto>();
            }
        }

        public async Task<TripSearchDetailsDto?> GetTripDetailsAsync(int tripId, DateTime journeyDate)
        {
            try
            {
                var trip = await _tripRepo.GetTripByIdAsync(tripId);
                if (trip == null) return null;

                var bus = await _busRepo.GetBusByIdAsync(trip.BusId);
                if (bus == null) return null;

                var operatorEntity = await _operatorRepo.GetOperatorByIdAsync(bus.OperatorId);
                if (operatorEntity == null) return null;

                int availableSeats = await GetAvailableSeatsAsync(tripId, journeyDate);

                return new TripSearchDetailsDto
                {
                    TripId = trip.Id,
                    OperatorName = operatorEntity.Name,
                    BusNo = bus.BusNo,
                    Departure = trip.DepartureTime,
                    Arrival = trip.ArrivalTime,
                    PricePerSeat = trip.Price,
                    SeatsAvailable = availableSeats,
                    TotalSeats = bus.TotalSeats,
                    BusType = bus.BusType
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTripDetailsAsync (Service): {ex.Message}");
                return null;
            }
        }


    }

}
