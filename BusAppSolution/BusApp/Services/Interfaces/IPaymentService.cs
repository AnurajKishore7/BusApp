using BusApp.DTOs;

namespace BusApp.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseDto>> GetAllAsync();
        Task<PaymentResponseDto?> GetByIdAsync(int paymentId);
        Task<PaymentResponseDto?> GetByBookingIdAsync(int bookingId);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, string status);
    }
}
