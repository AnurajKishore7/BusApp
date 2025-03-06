using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class ClientRegisterDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\+?[1-9][0-9]{9,14}$", ErrorMessage = "Invalid contact number format.")]
        public string Contact { get; set; }

        public bool IsHandicapped { get; set; } = false; 
    }

}
