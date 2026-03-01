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

        public UserRole Role { get; set; } = UserRole.Passenger;
    }
}