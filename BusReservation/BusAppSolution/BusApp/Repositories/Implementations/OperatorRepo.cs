using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class OperatorRepo : IOperatorRepo
    {
        private readonly AppDbContext _context;

        public OperatorRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransportOperator>> GetAllOperatorsAsync()
        {
            try
            {
                return await _context.TransportOperators.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllOperatorsAsync (Repo): {ex.Message}");
                return new List<TransportOperator>();
            }
        }

        public async Task<TransportOperator?> GetOperatorByIdAsync(int id)
        {
            try
            {
                return await _context.TransportOperators.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOperatorByIdAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<TransportOperator?> GetOperatorByEmailAsync(string email)
        {
            try
            {
                return await _context.TransportOperators.FirstOrDefaultAsync(c => c.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOperatorByEmailAsync (Repo): {ex.Message}");
                return null;
            }
        }


        public async Task<bool> UpdateOperatorAsync(int operatorId, UpdateOperatorDto updateDto)
        {
            try
            {
                var existingOperator = await _context.TransportOperators
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == operatorId);

                if (existingOperator == null) return false;

                // Updating fields
                existingOperator.Name = updateDto.Name;
                existingOperator.Contact = updateDto.Contact;

                if (existingOperator.User != null)
                {
                    existingOperator.User.Name = updateDto.Name;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateOperatorAsync (Repo): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOperatorAsync(string email)
        {
            try
            {
                var transportOperator = await _context.TransportOperators
                    .Include(c => c.User) // Load associated User entity
                    .FirstOrDefaultAsync(c => c.User.Email == email);

                if (transportOperator == null) return false; // TransportOperator not found

                // Mark as deleted
                transportOperator.IsDeleted = true;
                if (transportOperator.User != null)
                {
                    transportOperator.User.IsDeleted = true; // Also mark User as deleted
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteOperatorAsync (Repo): {ex.Message}");
                return false;
            }
        }
    }
}
