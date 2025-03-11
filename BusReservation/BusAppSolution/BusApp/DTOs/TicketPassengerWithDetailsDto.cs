using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class TicketPassengerWithDetailsDto
    {
        [Required(ErrorMessage = "Passenger name is required.")]
        public string PassengerName { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(1, 150, ErrorMessage = "Age must be between 1 and 150")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Seat number is required")]
        [StringLength(10, ErrorMessage = "Seat number cannot exceed 10 characters")]
        public string SeatNumber { get; set; } = "";
        public bool IsHandicapped { get; set; }
    }
}
