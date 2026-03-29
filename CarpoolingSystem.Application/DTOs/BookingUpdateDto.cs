using CarpoolingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class BookingUpdateDto {
        public BookingStatus? Status { get; set; }
    }
}
