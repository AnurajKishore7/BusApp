namespace BusApp.DTOs
{
    public class LoginResponseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public int ClientId { get; set; }
        public int TransportOperatorId { get; set; }

        public string Message {  get; set; }

        public string Token { get; set; }
    }

}
