using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideSessionUpdateDto {

        public LocationDto? Pickup { get; set; }
        public LocationDto? Destination { get; set; }

        [Range(1, 6, ErrorMessage = "Available seats must be between 1 and 6.")]
        public int? AvailableSeats { get; set; }

        public bool? IsActive { get; set; }
    }
}
