using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class TicketPassengerDto
    {
        [Required(ErrorMessage = "Passenger name is required.")]
        public string PassengerName { get; set; }
        public string Contact { get; set; }
        public bool IsHandicapped { get; set; }
    }
}
