namespace BusApp.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TripId { get; set; }
        public DateTime JourneyDate { get; set; }
        public int TicketCount { get; set; }
        public string Status { get; set; }

        // Details from TicketPassengers
        public List<TicketPassengerResponseDto> TicketPassengers { get; set; } = new List<TicketPassengerResponseDto>();

        // Additional properties
        public string ClientName { get; set; } = "";    // Fetched from Client table
        public string Source { get; set; } = "";    // Fetched from Trip table
        public string Destination { get; set; } = "";

        // Payment Details
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
    }

}
