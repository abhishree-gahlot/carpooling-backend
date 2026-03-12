using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideSessionUpdateDto {

        [StringLength(100, MinimumLength = 2)]
        public string? Pickup { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string? Dropoff { get; set; }

        [Range(1, 6, ErrorMessage = "Available seats must be between 1 and 6.")]
        public int? AvailableSeats { get; set; }

        public bool? IsActive { get; set; }
    }
}
