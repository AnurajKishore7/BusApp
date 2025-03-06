using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class BookingDto
    {
        [Required(ErrorMessage = "TripId is required.")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "Journey date is required.")]
        public DateTime JourneyDate { get; set; }

        [Required(ErrorMessage = "TicketCount is required.")]
        public int TicketCount { get; set; }

        [Required(ErrorMessage = "TicketPassenger is required.")]
        public List<TicketPassengerDto> TicketPassengers { get; set; } = new List<TicketPassengerDto>();
    }

}
