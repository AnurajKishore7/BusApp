using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class TicketPassenger
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Passenger name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^(\+91)?[0-9]\d{10}$", ErrorMessage = "Invalid Indian mobile number")]
        public string Contact { get; set; } = "";

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 150, ErrorMessage = "Age must be between 1 and 150")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        public string Gender { get; set; } = "";

        public bool IsHandicapped { get; set; } = false;
        [Required(ErrorMessage = "Seat number is required")]
        [StringLength(10, ErrorMessage = "Seat number cannot exceed 10 characters")]
        public string SeatNumber { get; set; } = "";


        //Navigation Property
        public Booking? Booking { get; set; }
    }
}
