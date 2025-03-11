namespace BusApp.DTOs
{
    public class TripDetailsResponseDto
    {
        public int Id { get; set; }
        public int BusRouteId { get; set; }
        public int BusId { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime JourneyDate { get; set; }
        public string[] AvailableSeats { get; set; }
    }
}
