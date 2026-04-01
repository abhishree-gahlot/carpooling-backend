using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs
{
    public class NotifyPassengerRejectedDto
    {
        public string PassengerId { get; set; } = string.Empty;
        public string RideRequestId { get; set; } = string.Empty;
    }
}
