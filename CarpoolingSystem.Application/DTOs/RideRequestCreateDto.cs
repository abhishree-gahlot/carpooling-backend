using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideRequestCreateDto {

        [Required]
        public Guid DriverId { get; set; }

        [Required]
        public LocationDto Pickup { get; set; } = new();

        [Required]
        public LocationDto Destination {  get; set; } = new();
    }
}
