using BusApp.DTOs;
using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface IOperatorRepo
    {
        Task<IEnumerable<TransportOperator>> GetAllOperatorsAsync();
        Task<TransportOperator?> GetOperatorByIdAsync(int id);
        Task<TransportOperator?> GetOperatorByEmailAsync(string email);
        Task<bool> UpdateOperatorAsync(int operatorId, UpdateOperatorDto updateDto);
        Task<bool> DeleteOperatorAsync(string email);
    }
}
