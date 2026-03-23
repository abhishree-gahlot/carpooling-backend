using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideSessionCreateDto {
        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public LocationDto Pickup { get; set; } = new();

        [Required]
        public LocationDto Destination { get; set; } = new();


        [Range(1, 6, ErrorMessage = "Available seats must be between 1 and 6.")]
        public int? AvailableSeats { get; set; }
    }
}
