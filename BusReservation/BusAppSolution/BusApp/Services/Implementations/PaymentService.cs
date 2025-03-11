using BusApp.DTOs;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;

namespace BusApp.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _repo;

        public PaymentService(IPaymentRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetAllAsync()
        {
            try
            {
                var payments = await _repo.GetAllAsync();
                return payments.Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    BookingId = p.BookingId,
                    TotalAmount = p.TotalAmount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync (Service): {ex.Message}");
                return new List<PaymentResponseDto>();
            }
        }

        public async Task<PaymentResponseDto?> GetByIdAsync(int paymentId)
        {
            try
            {
                var payment = await _repo.GetByIdAsync(paymentId);
                if (payment == null) return null;

                return new PaymentResponseDto
                {
                    Id = payment.Id,
                    BookingId = payment.BookingId,
                    TotalAmount = payment.TotalAmount,
                    PaymentMethod = payment.PaymentMethod,
                    Status = payment.Status
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<PaymentResponseDto?> GetByBookingIdAsync(int bookingId)
        {
            try
            {
                var payment = await _repo.GetByBookingIdAsync(bookingId);
                if (payment == null) return null;

                return new PaymentResponseDto
                {
                    Id = payment.Id,
                    BookingId = payment.BookingId,
                    TotalAmount = payment.TotalAmount,
                    PaymentMethod = payment.PaymentMethod,
                    Status = payment.Status
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByBookingIdAsync (Service): {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, string status, decimal newTotalAmount)
        {
            try
            {
                var payment = await _repo.GetByIdAsync(paymentId);
                if (payment == null) return false;

                return await _repo.UpdatePaymentStatusAsync(paymentId, status, newTotalAmount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePaymentStatusAsync (Service): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePaymentMethodAsync(int paymentId, string paymentMethod)
        {
            try
            {
                return await _repo.UpdatePaymentMethodAsync(paymentId, paymentMethod);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePaymentMethodAsync (Service): {ex.Message}");
                return false;
            }
        }

    }
}
