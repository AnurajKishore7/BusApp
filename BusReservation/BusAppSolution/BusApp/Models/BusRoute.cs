using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusApp.Models
{
    public class BusRoute
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Source location cannot exceed 100 characters.")]
        public string? Source { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Destination location cannot exceed 100 characters.")]
        public string? Destination { get; set; }

        [Required(ErrorMessage = "Estimated duration is required.")]
        [Column(TypeName = "time")]
        public TimeSpan EstimatedDuration { get; set; }  // e.g., "14:30" (total hours of journey)

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Distance must be a positive integer.")]
        public int Distance { get; set; }

        public bool IsDeleted { get; set; } = false;


        //Navigation Property
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
