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

        public bool IsHandicapped { get; set; } = false;


        //Navigation Property
        public Booking? Booking { get; set; }
    }
}
