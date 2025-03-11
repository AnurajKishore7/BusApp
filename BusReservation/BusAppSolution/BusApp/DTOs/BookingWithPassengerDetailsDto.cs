using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class BookingWithPassengerDetailsDto
    {
        [Required(ErrorMessage = "TripId is required.")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "Journey date is required.")]
        public DateTime JourneyDate { get; set; }

        [Required(ErrorMessage = "TicketCount is required.")]
        public int TicketCount { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^(\+91\d{10})$|^(\d{10})$", ErrorMessage = "Invalid Indian mobile number (10 digits, or 10 digits with +91)")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "TicketPassengers are required.")]
        public List<TicketPassengerWithDetailsDto> TicketPassengers { get; set; } = new List<TicketPassengerWithDetailsDto>();
    }
}
