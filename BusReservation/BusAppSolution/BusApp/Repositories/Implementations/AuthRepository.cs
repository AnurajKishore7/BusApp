using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;


namespace BusApp.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        public readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> RegisterClient(User user, Client client)
        {
            if (await UserExists(user.Email))
                throw new InvalidOperationException("Email is already registered.");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> RegisterTransportOperator(User user)
        {
            if (await UserExists(user.Email))
                throw new InvalidOperationException("Email is already registered.");

            user.IsApproved = false; // Needs admin approval
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<List<User>> GetUsersByRoleAndApproval(string role, bool isApproved)
        {
            return await _context.Users
              .Where(u => u.Role == role && u.IsApproved == isApproved && !u.IsDeleted)
              .ToListAsync();
        }

        public async Task<bool> ApproveTransportOperator(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name && u.Role == "TransportOperator");

            if (user == null || user.IsApproved || user.IsDeleted)
                return false;

            user.IsApproved = true; // Mark user as approved
            await _context.SaveChangesAsync();

            var transportOperator = new TransportOperator
            {
                Name = user.Name,
                Email = user.Email,
                Contact = "0000000000", // Default if not provided
                IsDeleted = false
            };

            await _context.TransportOperators.AddAsync(transportOperator);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
