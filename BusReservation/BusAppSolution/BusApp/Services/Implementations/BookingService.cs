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
                    Contact = booking.Contact,
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
                    Contact = booking.Contact,
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
                        Age = p.Age,
                        Gender = p.Gender,
                        SeatNumber = p.SeatNumber,
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
                    Contact = b.Contact,
                    TicketPassengers = b.TicketPassengers?.Select(tp => new TicketPassengerResponseDto
                    {
                        Id = tp.Id,
                        BookingId = tp.BookingId,
                        PassengerName = tp.Name,
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
                if (totalSeats != 40)
                {
                    throw new Exception("Expected 40 seats for the bus.");
                }

                // Get currently booked seat numbers
                var bookedSeatNumbers = await _bookingRepo.GetBookedSeatNumbersAsync(bookingDto.TripId, bookingDto.JourneyDate);

                // Extract selected seat numbers from TicketPassengers
                var selectedSeatNumbers = bookingDto.TicketPassengers.Select(tp => tp.SeatNumber).ToList();
                if (selectedSeatNumbers.Count != bookingDto.TicketCount)
                {
                    throw new Exception("Number of selected seats must match TicketCount.");
                }

                // Validate that selected seats are not already booked
                var duplicateSeats = selectedSeatNumbers.Intersect(bookedSeatNumbers).ToList();
                if (duplicateSeats.Any())
                {
                    throw new Exception($"The following seats are already booked: {string.Join(", ", duplicateSeats)}");
                }

                // Validate that all selected seat numbers are valid (A1-A20, B1-B20)
                var validSeatNumbers = new List<string>();
                for (int i = 1; i <= 20; i++) validSeatNumbers.Add($"A{i}");
                for (int i = 1; i <= 20; i++) validSeatNumbers.Add($"B{i}");
                var invalidSeats = selectedSeatNumbers.Except(validSeatNumbers).ToList();
                if (invalidSeats.Any())
                {
                    throw new Exception($"Invalid seat numbers: {string.Join(", ", invalidSeats)}");
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
                    Contact = bookingDto.Contact,
                    Status = "Pending"
                };

                var addedBooking = await _bookingRepo.AddBookingAsync(newBooking);

                // 5. Add TicketPassengers with SeatNumbers
                var passengers = bookingDto.TicketPassengers.Select(p => new TicketPassenger
                {
                    BookingId = addedBooking.Id,
                    Name = p.PassengerName,
                    Age = p.Age,
                    Gender = p.Gender,
                    IsHandicapped = p.IsHandicapped,
                    SeatNumber = p.SeatNumber
                }).ToList();

                await _ticketPassengerRepo.AddPassengersAsync(passengers);

                // Reload the saved passengers from the database
                var savedPassengers = await _ticketPassengerRepo.GetByBookingIdAsync(addedBooking.Id);
                if (!savedPassengers.Any())
                {
                    throw new Exception("Failed to save ticket passengers.");
                }

                // 6. Calculate Total Amount with Breakdown
                decimal baseFare = trip.Price * bookingDto.TicketCount;
                decimal gst = baseFare * 0.06m;
                decimal convenienceFee = 10m;
                decimal totalAmount = baseFare + gst + convenienceFee;

                // 7. Create a Payment entry with "Pending" status
                var payment = new Payment
                {
                    BookingId = addedBooking.Id,
                    TotalAmount = totalAmount,
                    PaymentMethod = "Online",
                    Status = "Pending"
                };

                var addedPayment = await _paymentRepo.AddPaymentAsync(payment);

                var response = new BookingResponseDto
                {
                    Id = addedBooking.Id,
                    ClientId = client.Id,
                    TripId = bookingDto.TripId,
                    JourneyDate = bookingDto.JourneyDate,
                    TicketCount = bookingDto.TicketCount,
                    Status = addedBooking.Status,
                    Contact = addedBooking.Contact,
                    ClientName = client.Name,
                    Source = busRoute.Source,
                    Destination = busRoute.Destination,
                    BaseFare = baseFare,
                    GST = gst,
                    ConvenienceFee = convenienceFee,
                    TotalAmount = totalAmount,
                    PaymentMethod = payment.PaymentMethod,
                    PaymentStatus = payment.Status,
                    TicketPassengers = savedPassengers.Select(p => new TicketPassengerResponseDto
                    {
                        Id = p.Id,
                        BookingId = p.BookingId,
                        PassengerName = p.Name,
                        Age = p.Age,
                        Gender = p.Gender,
                        SeatNumber = p.SeatNumber,
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
                var trips = await _tripRepo.GetTripsBySourceAndDestinationAsync(source, destination);
                var tripDetailsList = new List<TripSearchDetailsDto>();

                foreach (var trip in trips)
                {
                    var bus = await _busRepo.GetBusByIdAsync(trip.BusId);
                    var operatorEntity = bus != null ? await _operatorRepo.GetOperatorByIdAsync(bus.OperatorId) : null;
                    int availableSeats = await GetAvailableSeatsAsync(trip.Id, journeyDate);

                    tripDetailsList.Add(new TripSearchDetailsDto
                    {
                        TripId = trip.Id,
                        OperatorName = operatorEntity?.Name ?? "Unknown Operator",
                        BusNo = bus?.BusNo ?? "Unknown Bus",
                        Departure = trip.DepartureTime,
                        Arrival = trip.ArrivalTime,
                        PricePerSeat = trip.Price,
                        SeatsAvailable = availableSeats,
                        TotalSeats = bus?.TotalSeats ?? 0,
                        BusType = bus?.BusType ?? "Unknown Type"
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
                var operatorEntity = bus != null ? await _operatorRepo.GetOperatorByIdAsync(bus.OperatorId) : null;
                int availableSeats = await GetAvailableSeatsAsync(tripId, journeyDate);

                return new TripSearchDetailsDto
                {
                    TripId = trip.Id,
                    OperatorName = operatorEntity?.Name ?? "Unknown Operator",
                    BusNo = bus?.BusNo ?? "Unknown Bus",
                    Departure = trip.DepartureTime,
                    Arrival = trip.ArrivalTime,
                    PricePerSeat = trip.Price,
                    SeatsAvailable = availableSeats,
                    TotalSeats = bus?.TotalSeats ?? 0,
                    BusType = bus?.BusType ?? "Unknown Type"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTripDetailsAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<SeatLayoutDto> GetSeatLayoutAsync(int tripId, DateTime journeyDate)
        {
            try
            {
                var trip = await _tripRepo.GetTripByIdAsync(tripId);
                if (trip == null)
                {
                    throw new Exception("Trip not found.");
                }

                // Get total seats for the bus
                int totalSeats = await _busRepo.GetTotalSeatsByBusIdAsync(trip.BusId);
                if (totalSeats != 40)
                {
                    throw new Exception("Expected 40 seats for the bus.");
                }

                // Generate seat numbers (A1-A20, B1-B20)
                var seatNumbers = new List<string>();
                for (int i = 1; i <= 20; i++)
                {
                    seatNumbers.Add($"A{i}");
                }
                for (int i = 1; i <= 20; i++)
                {
                    seatNumbers.Add($"B{i}");
                }

                // Get booked seat numbers
                var bookedSeatNumbers = await _bookingRepo.GetBookedSeatNumbersAsync(tripId, journeyDate);

                var seats = seatNumbers.Select(seatNumber => new SeatDto
                {
                    SeatNumber = seatNumber,
                    IsBooked = bookedSeatNumbers.Contains(seatNumber)
                }).ToList();

                return new SeatLayoutDto
                {
                    TripId = tripId,
                    JourneyDate = journeyDate,
                    TotalSeats = totalSeats,
                    Seats = seats
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetSeatLayoutAsync for TripId {tripId} on {journeyDate}: {ex.Message}");
                throw;
            }
        }
    }
}
