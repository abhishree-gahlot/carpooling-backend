using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideSessionCreateDto {
        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Pickup { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Dropoff { get; set; } = string.Empty;

        
        [Range(1, 6, ErrorMessage = "Available seats must be between 1 and 6.")]
        public int? AvailableSeats { get; set; }
    }
}
