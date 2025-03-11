namespace BusApp.DTOs
{
    public class BusDto
    {
        public int Id { get; set; }
        public string? BusNo { get; set; }
        public int OperatorId { get; set; }
        public string? BusType { get; set; }
        public int TotalSeats { get; set; }
    }
}
