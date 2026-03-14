using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideRequestUpdateDto {
        [StringLength(100, MinimumLength = 2)]
        public string? Pickup { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string? Destination { get; set; }

        public RideRequestStatus? RideRequestStatus { get; set; }

    }
}
