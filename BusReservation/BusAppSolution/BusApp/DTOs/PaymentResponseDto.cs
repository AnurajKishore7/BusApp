namespace BusApp.DTOs
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } // Pending, Completed, Failed
    }

}
