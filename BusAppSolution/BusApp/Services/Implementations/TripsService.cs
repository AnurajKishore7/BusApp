using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;

namespace BusApp.Services.Implementations
{
    public class TripsService : ITripsService
    {
        private readonly ITripsRepo _tripsRepo;

        public TripsService(ITripsRepo tripsRepo)
        {
            _tripsRepo = tripsRepo;
        }

        public async Task<IEnumerable<TripResponseDto>> GetAllTripsAsync()
        {
            try
            {
                var trips = await _tripsRepo.GetAllTripsAsync();

                return trips.Select(trip => new TripResponseDto
                {
                    Id = trip.Id,
                    BusRouteId = trip.BusRouteId,
                    BusId = trip.BusId,
                    DepartureTime = trip.DepartureTime,
                    ArrivalTime = trip.ArrivalTime,
                    Price = trip.Price
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllTripsAsync (Service): {ex.Message}");
                return new List<TripResponseDto>();
            }
        }

        public async Task<TripResponseDto?> GetTripByIdAsync(int id)
        {
            try
            {
                var trip = await _tripsRepo.GetTripByIdAsync(id);

                if (trip == null) return null;

                return new TripResponseDto
                {
                    Id = trip.Id,
                    BusRouteId = trip.BusRouteId,
                    BusId = trip.BusId,
                    DepartureTime = trip.DepartureTime,
                    ArrivalTime = trip.ArrivalTime,
                    Price = trip.Price
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTripByIdAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<TripResponseDto>> GetTripsByRouteAsync(int busRouteId)
        {
            try
            {
                var trips = await _tripsRepo.GetTripsByRouteAsync(busRouteId);

                return trips.Select(trip => new TripResponseDto
                {
                    Id = trip.Id,
                    BusRouteId = trip.BusRouteId,
                    BusId = trip.BusId,
                    DepartureTime = trip.DepartureTime,
                    ArrivalTime = trip.ArrivalTime,
                    Price = trip.Price
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTripsByRouteAsync (Service): {ex.Message}");
                return new List<TripResponseDto>();
            }
        }

        public async Task<IEnumerable<TripResponseDto>> GetTripsBySourceAndDestinationAsync(string source, string destination)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
                {
                    throw new ArgumentException("Source and destination cannot be empty or whitespace.");
                }

                var trips = await _tripsRepo.GetTripsBySourceAndDestinationAsync(source.Trim(), destination.Trim());

                return trips.Select(trip => new TripResponseDto
                {
                    Id = trip.Id,
                    BusRouteId = trip.BusRouteId,
                    BusId = trip.BusId,
                    DepartureTime = trip.DepartureTime,
                    ArrivalTime = trip.ArrivalTime,
                    Price = trip.Price
                }).ToList();
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"Validation Error in GetTripsBySourceAndDestinationAsync (Service): {argEx.Message}");
                return new List<TripResponseDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTripsBySourceAndDestinationAsync (Service): {ex.Message}");
                return new List<TripResponseDto>();
            }
        }

        public async Task<TripResponseDto?> AddTripAsync(TripDto dto)
        {
            try
            {
                // Validate input DTO
                if (dto == null)
                {
                    throw new ArgumentException("Trip data cannot be null.");
                }

                //if (dto.DepartureTime >= dto.ArrivalTime)
                //{
                //    throw new ArgumentException("Departure time must be earlier than arrival time.");
                //}

                var trip = new Trip
                {
                    BusRouteId = dto.BusRouteId,
                    BusId = dto.BusId,
                    DepartureTime = dto.DepartureTime,
                    ArrivalTime = dto.ArrivalTime,
                    Price = dto.Price
                };

                var addedTrip = await _tripsRepo.AddTripAsync(trip);
                if (addedTrip == null)
                {
                    throw new Exception("Failed to add the trip.");
                }

                return new TripResponseDto
                {
                    Id = addedTrip.Id,
                    BusRouteId = addedTrip.BusRouteId,
                    BusId = addedTrip.BusId,
                    DepartureTime = addedTrip.DepartureTime,
                    ArrivalTime = addedTrip.ArrivalTime,
                    Price = addedTrip.Price
                };
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"Validation Error in AddTripAsync (Service): {argEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddTripAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<TripResponseDto?> UpdateTripAsync(int id, TripDto dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new ArgumentException("Trip data cannot be null.");
                }

                if (dto.DepartureTime >= dto.ArrivalTime)
                {
                    throw new ArgumentException("Departure time must be earlier than arrival time.");
                }

                // Fetch the existing trip
                var existingTrip = await _tripsRepo.GetTripByIdAsync(id);
                if (existingTrip == null)
                {
                    throw new KeyNotFoundException($"Trip with ID {id} not found.");
                }

                // Update the trip properties
                existingTrip.BusRouteId = dto.BusRouteId;
                existingTrip.BusId = dto.BusId;
                existingTrip.DepartureTime = dto.DepartureTime;
                existingTrip.ArrivalTime = dto.ArrivalTime;
                existingTrip.Price = dto.Price;

                var updatedTrip = await _tripsRepo.UpdateTripAsync(existingTrip);
                if (updatedTrip == null)
                {
                    throw new Exception("Failed to update the trip.");
                }

                return new TripResponseDto
                {
                    Id = updatedTrip.Id,
                    BusRouteId = updatedTrip.BusRouteId,
                    BusId = updatedTrip.BusId,
                    DepartureTime = updatedTrip.DepartureTime,
                    ArrivalTime = updatedTrip.ArrivalTime,
                    Price = updatedTrip.Price
                };
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"Validation Error in UpdateTripAsync (Service): {argEx.Message}");
                return null;
            }
            catch (KeyNotFoundException keyEx)
            {
                Console.WriteLine($"Not Found Error in UpdateTripAsync (Service): {keyEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateTripAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            try
            {
                return await _tripsRepo.DeleteTripAsync(id);
            }
            catch (KeyNotFoundException keyEx)
            {
                Console.WriteLine($"Not Found Error in DeleteTripAsync (Service): {keyEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteTripAsync (Service): {ex.Message}");
                return false;
            }
        }
    }
}
