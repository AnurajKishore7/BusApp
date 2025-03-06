using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly AppDbContext _context;

        public PaymentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            try
            {
                return await _context.Payments.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync (Repo): {ex.Message}");
                return new List<Payment>();
            }
        }

        public async Task<Payment?> GetByIdAsync(int paymentId)
        {
            try
            {
                return await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<Payment?> GetByBookingIdAsync(int bookingId)
        {
            try
            {
                return await _context.Payments.FirstOrDefaultAsync(p => p.BookingId == bookingId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByBookingIdAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddPaymentAsync (Repo): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, string status, decimal totalAmount)
        {
            try
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
                if (payment == null)
                    return false;

                if (payment.Status.Equals(status, StringComparison.OrdinalIgnoreCase) && payment.TotalAmount == totalAmount)
                {
                    return true;
                }

                payment.Status = status;
                payment.TotalAmount = totalAmount;

                if (status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                {
                    var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == payment.BookingId);
                    if (booking != null)
                    {
                        booking.Status = "Confirmed";
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePaymentStatusAsync (Repo): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePaymentMethodAsync(int paymentId, string paymentMethod)
        {
            try
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
                if (payment == null) return false;

                payment.PaymentMethod = paymentMethod;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePaymentMethodAsync (Repo): {ex.Message}");
                return false;
            }
        }

    }
}
