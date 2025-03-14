﻿using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class BusRouteDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Source location cannot exceed 100 characters.")]
        public string? Source { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Destination location cannot exceed 100 characters.")]
        public string? Destination { get; set; }

        [Required]
        [RegularExpression(@"^\d{1,2}:\d{2}$", ErrorMessage = "Estimated duration must be in the format HH:mm.")]
        public string? EstimatedDuration { get; set; }  // e.g., "16:30"

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Distance must be a positive integer.")]
        public int Distance { get; set; }
    }
}
