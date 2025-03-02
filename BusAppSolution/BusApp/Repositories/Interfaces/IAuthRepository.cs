using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> UserExists(string email);
        Task<User?> GetUserByEmail(string email);
        Task<User> RegisterClient(User user, Client client);
        Task<User> RegisterTransportOperator(User user);
        Task<bool> ApproveTransportOperator(string name);
        Task<List<User>> GetUsersByRoleAndApproval(string role, bool isApproved);
    }

}
