using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;

namespace BusApp.Services.Implementations
{
    public class BusesService : IBusesService
    {
        private readonly IBusesRepo _busesRepo;
        private readonly IOperatorRepo _operatorRepo;

        public BusesService(IBusesRepo busesRepo, IOperatorRepo operatorRepo)
        {
            _busesRepo = busesRepo;
            _operatorRepo = operatorRepo;
        }

        public async Task<IEnumerable<BusDto>> GetAllBusesAsync()
        {
            try
            {
                var buses = await _busesRepo.GetAllBusesAsync();
                return buses.Select(b => new BusDto
                {
                    Id = b.Id,
                    BusNo = b.BusNo,
                    OperatorId = b.OperatorId,
                    BusType = b.BusType,
                    TotalSeats = b.TotalSeats
                    
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllBusesAsync (Service): {ex.Message}");
                return new List<BusDto>();
            }
        }

        public async Task<IEnumerable<BusDto>> GetMyBusesAsync(string email)
        {
            try
            {
                // Fetch the operator details using the email
                var operatorEntity = await _operatorRepo.GetOperatorByEmailAsync(email);
                if (operatorEntity == null)
                {
                    Console.WriteLine("Error: Operator not found.");
                    return new List<BusDto>();
                }

                // Fetch only the buses associated with this operator
                var buses = await _busesRepo.GetMyBusesAsync(operatorEntity.Id);

                return buses.Select(b => new BusDto
                {
                    Id = b.Id,
                    BusNo = b.BusNo,
                    BusType = b.BusType,
                    TotalSeats = b.TotalSeats,
                    OperatorId = b.OperatorId
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyBusesAsync (Service): {ex.Message}");
                return new List<BusDto>();
            }
        }

        public async Task<BusDto?> GetBusByIdAsync(int id, string role, string email)
        {
            try
            {
                Bus? bus = null;

                if (role == "Admin")
                {
                    // Admin can directly fetch any bus by ID
                    bus = await _busesRepo.GetBusByIdAsync(id);
                }
                else if (role == "TransportOperator")
                {
                    // Fetch operator details using email
                    var operatorEntity = await _operatorRepo.GetOperatorByEmailAsync(email);
                    if (operatorEntity == null)
                    {
                        Console.WriteLine("Error: Operator not found.");
                        return null;
                    }

                    // Fetch only if the bus belongs to the transport operator
                    var myBuses = await _busesRepo.GetMyBusesAsync(operatorEntity.Id);
                    bus = myBuses.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
                }

                if (bus == null) return null;

                return new BusDto
                {
                    Id = bus.Id,
                    BusNo = bus.BusNo,
                    BusType = bus.BusType,
                    TotalSeats = bus.TotalSeats,
                    OperatorId = bus.OperatorId
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBusByIdAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddBusAsync(NewBusDto newBusDto, string email)
        {
            try
            {
                // Fetch the operator's ID from the email
                var transportOperator = await _operatorRepo.GetOperatorByEmailAsync(email);

                if (transportOperator == null) return false;

                var newBus = new Bus
                {
                    BusNo = newBusDto.BusNo,
                    BusType = newBusDto.BusType,
                    TotalSeats = newBusDto.TotalSeats,
                    OperatorId = transportOperator.Id, // Assign fetched operator ID
                    IsDeleted = false
                };

                return await _busesRepo.AddBusAsync(newBus);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddBusAsync (Service): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateBusAsync(string email, int busId, UpdateBusDto updateBusDto)
        {
            try
            {
                // Fetch the operator's ID from the email
                var operatorEntity = await _operatorRepo.GetOperatorByEmailAsync(email);
                if (operatorEntity == null) return false;

                // Check if the bus exists and belongs to the operator
                var bus = await _busesRepo.GetBusByIdAsync(busId);
                if (bus == null || bus.OperatorId != operatorEntity.Id) return false;

                return await _busesRepo.UpdateBusAsync(busId, updateBusDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBusAsync (Service): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBusAsync(string email, int busId)
        {
            try
            {
                // Fetch the operator's ID from the email
                var operatorEntity = await _operatorRepo.GetOperatorByEmailAsync(email);
                if (operatorEntity == null) return false;

                // Check if the bus exists and belongs to the operator
                var bus = await _busesRepo.GetBusByIdAsync(busId);
                if (bus == null || bus.OperatorId != operatorEntity.Id || bus.IsDeleted) return false;

                return await _busesRepo.DeleteBusAsync(busId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteBusAsync (Service): {ex.Message}");
                return false;
            }
        }
    }
}
