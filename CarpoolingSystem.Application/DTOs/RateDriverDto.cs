using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarpoolingSystem.Application.DTOs {
    public class RateDriverDto {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public decimal Ratings { get; set; }
    }
}
