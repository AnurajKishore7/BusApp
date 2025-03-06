using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class ClientRepo : IClientRepo
    {
        private readonly AppDbContext _context;

        public ClientRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            try
            {
                return await _context.Clients.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllClientsAsync (Repo): {ex.Message}");
                return new List<Client>();
            }
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            try
            {
                return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientByIdAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<Client?> GetClientByEmailAsync(string email)
        {
            try
            {
                return await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientByEmailAsync (Repo): {ex.Message}");
                return null;
            }
        }


        public async Task<bool> UpdateClientAsync(int clientId, UpdateClientDto updateDto)
        {
            try
            {
                var existingClient = await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == clientId);

                if (existingClient == null) return false;

                // Updating fields
                existingClient.Name = updateDto.Name;
                existingClient.DOB = updateDto.DOB;
                existingClient.Gender = updateDto.Gender;
                existingClient.Contact = updateDto.Contact;
                existingClient.IsHandicapped = updateDto.IsHandicapped;

                if (existingClient.User != null)
                {
                    existingClient.User.Name = updateDto.Name;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateClientAsync (Repo): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteClientAsync(string email)
        {
            try
            {
                var client = await _context.Clients
                    .Include(c => c.User) // Load associated User entity
                    .FirstOrDefaultAsync(c => c.User.Email == email);

                if (client == null) return false; // Client not found

                // Mark as deleted
                client.IsDeleted = true;
                if (client.User != null)
                {
                    client.User.IsDeleted = true; // Also mark User as deleted
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteClientAsync (Repo): {ex.Message}");
                return false;
            }
        }

    }
}
