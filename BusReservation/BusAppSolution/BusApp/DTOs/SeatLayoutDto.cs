namespace BusApp.DTOs
{
    public class SeatLayoutDto
    {
        public int TripId { get; set; }
        public DateTime JourneyDate { get; set; }
        public int TotalSeats { get; set; }
        public List<SeatDto> Seats { get; set; } = new List<SeatDto>();
    }

    public class SeatDto
    {
        public string SeatNumber { get; set; } = "";
        public bool IsBooked { get; set; }
    }
}
