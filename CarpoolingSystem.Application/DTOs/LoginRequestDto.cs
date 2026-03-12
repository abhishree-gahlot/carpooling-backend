using System.ComponentModel.DataAnnotations;

namespace CarpoolingSystem.Application.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number and special character.")]
        public string Password { get; set; } = null!;
    }
}