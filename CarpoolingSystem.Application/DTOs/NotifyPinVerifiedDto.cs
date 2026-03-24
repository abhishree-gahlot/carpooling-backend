using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs
{
    public class NotifyPinVerifiedDto
    {
        public String PassengerId { get; set; }
        public bool Success { get; set; }
    }
}
