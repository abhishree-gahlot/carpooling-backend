using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs
{
    public class PaymentMadeDto
    {
        public Guid DriverId { get; set; }
        public Guid RideRequestId { get; set; }
    }
}
