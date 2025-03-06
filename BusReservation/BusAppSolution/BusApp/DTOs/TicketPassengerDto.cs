using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class TicketPassengerDto
    {
        [Required(ErrorMessage = "Passenger name is required.")]
        public string PassengerName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Contact { get; set; }
        [Required(ErrorMessage = "Seat number is required")]
        [StringLength(10, ErrorMessage = "Seat number cannot exceed 10 characters")]
        public string SeatNumber { get; set; } = "";
        public bool IsHandicapped { get; set; }
    }
}
