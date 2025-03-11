namespace BusApp.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TripId { get; set; }
        public DateTime JourneyDate { get; set; }
        public int TicketCount { get; set; }
        public string Status { get; set; } = "Pending";
        public string Contact { get; set; } = "";
        public List<TicketPassengerResponseDto> TicketPassengers { get; set; } = new List<TicketPassengerResponseDto>();
        public string ClientName { get; set; } = "";
        public string Source { get; set; } = "";
        public string Destination { get; set; } = "";
        public decimal BaseFare { get; set; }
        public decimal GST { get; set; }
        public decimal ConvenienceFee { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
    }
}
