﻿namespace BusApp.DTOs
{
    public class TicketPassengerResponseDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string PassengerName { get; set; }
        public string? Contact { get; set; }
        public bool IsHandicapped { get; set; }
    }

}
