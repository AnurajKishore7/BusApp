using BusApp.Models;

namespace BusApp.Repositories.Interfaces
{
    public interface IPaymentRepo
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int paymentId);
        Task<Payment?> GetByBookingIdAsync(int bookingId);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, string status, decimal totalAmount);
        Task<bool> UpdatePaymentMethodAsync(int paymentId, string paymentMethod);
    }

}
