using System.ComponentModel.DataAnnotations;
using CarpoolingSystem.Domain.Enums;

namespace CarpoolingSystem.Application.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number and special character.")]
        public string Password { get; set; } = null!;

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Za-z ]{2,}$",
        ErrorMessage = "Username must contain only letters and spaces.")]
        public string Username { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; } = UserRole.Passenger;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string? VehicleName { get; set; }

        [Range(1, 6, ErrorMessage = "Vehicle must have between 1 and 6 seats.")]
        public int? MaxSeats { get; set; }

        public string? VehicleLicense { get; set; }
        public string? DriverLicenseFile { get; set; }
        public string? DriverLicenseFileName { get; set; }
    }
}