namespace BusApp.DTOs
{
    public class TripResponseDto
    {
        public int Id { get; set; }
        public int BusRouteId { get; set; }
        public int BusId { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public decimal Price { get; set; }
    }

}
