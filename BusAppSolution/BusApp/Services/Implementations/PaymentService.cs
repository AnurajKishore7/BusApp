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

        public async Task<bool> UpdatePaymentStatusAsync(int bookingId, string status)
        {
            try
            {
                return await _repo.UpdatePaymentStatusAsync(bookingId, status);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePaymentStatusAsync (Service): {ex.Message}");
                return false;
            }
        }

    }
}
