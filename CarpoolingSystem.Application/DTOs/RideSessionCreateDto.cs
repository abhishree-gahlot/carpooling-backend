using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideSessionCreateDto {
        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(500)]
        public string Pickup { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [MaxLength(500)]
        public string Dropoff { get; set; } = string.Empty;

        
        [Range(1, 6, ErrorMessage = "Available seats must be between 1 and 6.")]
        public int? AvailableSeats { get; set; }
    }
}
