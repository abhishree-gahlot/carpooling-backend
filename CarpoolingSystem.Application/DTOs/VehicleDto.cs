using System.ComponentModel.DataAnnotations;

namespace CarpoolingSystem.Application.DTOs
{
    public class VehicleDTO
    {
        public Guid VehicleId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string VehicleName { get; set; } = string.Empty;

        public Guid DriverId { get; set; }

        public string DriverName { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Vehicle must have between 1 and 6 seats.")]
        public int MaxSeats { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-Z0-9-]+$", ErrorMessage = "Invalid license plate format.")]
        public string LicensePlate { get; set; } = string.Empty;
    }


    public class VehicleCreateDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string VehicleName { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Vehicle must have between 1 and 6 seats.")]
        public int MaxSeats { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-Z0-9-]+$", ErrorMessage = "Invalid license plate format.")]
        public string LicensePlate { get; set; } = string.Empty;
    }


    public class VehicleUpdateDTO
    {
        [StringLength(50, MinimumLength = 2)]
        public string? VehicleName { get; set; }

        [Range(1, 6, ErrorMessage = "Vehicle must have between 1 and 6 seats.")]
        public int? MaxSeats { get; set; }

        public bool? IsActive { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[A-Z0-9-]+$", ErrorMessage = "Invalid license plate format.")]
        public string? LicensePlate { get; set; }
    }
}