using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class BusSeat
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Bus")]
        public int BusId { get; set; }

        [Required(ErrorMessage = "Seat number is required")]
        [StringLength(10, ErrorMessage = "Seat number cannot exceed 10 characters")]
        public string SeatNumber { get; set; } = ""; // e.g., "A1", "B2"

        public bool IsDeleted { get; set; } = false;

        // Navigation Property
        public Bus? Bus { get; set; }
    }
}
