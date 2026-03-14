using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideRequestCreateDto {

        [Required]
        [StringLength(100, MinimumLength =2)]
        public string Pickup { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Destination {  get; set; } = string.Empty;
    }
}
