namespace BusApp.DTOs
{
    public class BusRouteResponseDto
    {
        public int Id { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public TimeSpan EstimatedDuration { get; set; }  // e.g., "16:30" as TimeSpan
        public int Distance { get; set; }
    }
}