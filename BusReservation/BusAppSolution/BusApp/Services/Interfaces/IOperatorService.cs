using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
    public interface IOperatorService
    {
        Task<IEnumerable<OperatorDto>> GetAllOperatorsAsync();
        Task<OperatorDto?> GetOperatorByIdAsync(int id);
        Task<OperatorDto?> GetOperatorByEmailAsync(string email);
        Task<OperatorDto?> GetOperatorAsync();
        Task<bool> UpdateOperatorAsync(UpdateOperatorDto updateDto);
        Task<bool> DeleteOperatorAsync();
        Task<bool> DeleteOperatorAsync(string email);
    }
}
