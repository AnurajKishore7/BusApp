using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class TripDto
    {
        [Required(ErrorMessage = "Bus route ID is required.")]
        public int BusRouteId { get; set; }

        [Required(ErrorMessage = "Bus ID is required.")]
        public int BusId { get; set; }

        [Required(ErrorMessage = "Departure time is required.")]
        public TimeSpan DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival time is required.")]
        public TimeSpan ArrivalTime { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }
    }

}
