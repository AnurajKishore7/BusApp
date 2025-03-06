namespace BusApp.DTOs
{
    public class TripSearchDetailsDto
    {
        public int TripId { get; set; }
        public string OperatorName { get; set; }
        public string BusNo { get; set; }
        public TimeSpan Departure { get; set; }
        public TimeSpan Arrival { get; set; }
        public decimal PricePerSeat { get; set; }
        public int SeatsAvailable { get; set; }
        public int TotalSeats { get; set; }
        public string BusType { get; set; }
    }
}
