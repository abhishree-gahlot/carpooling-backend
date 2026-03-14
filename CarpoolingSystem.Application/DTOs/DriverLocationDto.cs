using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs
{
    public class DriverLocationDto
    {
        public Guid DriverId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
