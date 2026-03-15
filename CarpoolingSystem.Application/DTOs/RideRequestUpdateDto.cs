using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RideRequestUpdateDto {
        public RideRequestStatus? RideRequestStatus { get; set; }

    }
}
