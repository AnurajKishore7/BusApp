using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "Journey date is required.")]
        [Column(TypeName = "date")]
        public DateTime JourneyDate { get; set; }

        [Required(ErrorMessage = "Number of tickets is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "At least one ticket must be booked.")]
        public int TicketCount { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; } = "Pending"; // Pending | Confirmed | Cancelled
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime BookedOn { get; set; } = DateTime.Now;


        // Navigation Properties
        public Client? Client { get; set; }
        public Trip? Trip { get; set; }
        public ICollection<TicketPassenger> TicketPassengers { get; set; } = new List<TicketPassenger>();
        public Payment? Payment { get; set; }
    }

}
