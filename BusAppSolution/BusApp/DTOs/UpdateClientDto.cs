using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class UpdateClientDto
        {
            [Required(ErrorMessage = "Name is required.")]
            [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
            [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
            public string Name { get; set; } = string.Empty;

            public DateOnly? DOB { get; set; }

            public string? Gender { get; set; }

            [Required(ErrorMessage = "Contact number is required.")]
            [RegularExpression(@"^\+?[1-9][0-9]{9,14}$", ErrorMessage = "Invalid contact number format.")]
            public string? Contact { get; set; }

            public bool IsHandicapped { get; set; } = false;
        }
}
