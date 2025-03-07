using System.Text.Json.Serialization;
using BusApp.Converters;

namespace BusApp.Models
{
    public class TripSearchDetailsDto
    {
        public int TripId { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Departure { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Arrival { get; set; }
        public string OperatorName { get; set; }
        public string BusNo { get; set; }
        public decimal PricePerSeat { get; set; }
        public int SeatsAvailable { get; set; }
        public int TotalSeats { get; set; }
        public string BusType { get; set; }
    }
}