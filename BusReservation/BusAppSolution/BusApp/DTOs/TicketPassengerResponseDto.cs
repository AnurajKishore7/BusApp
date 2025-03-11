namespace BusApp.DTOs
{
    public class TicketPassengerResponseDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string PassengerName { get; set; } = "";
        public int Age { get; set; }
        public string Gender { get; set; } = "";
        public string SeatNumber { get; set; } = "";
        public bool IsHandicapped { get; set; }
    }
}
