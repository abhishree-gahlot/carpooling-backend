using System.ComponentModel.DataAnnotations;

namespace CarpoolingSystem.Application.DTOs
{
    public class VehicleDTO
    {
        public Guid VehicleId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string VehicleName { get; set; } = string.Empty;

        [Required]
        public Guid DriverId { get; set; }

        public string DriverName { get; set; } = string.Empty;

        [Range(1, 6)]
        public int MaxSeats { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;
    }

    public class VehicleCreateDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string VehicleName { get; set; } = string.Empty;

        [Required]
        public Guid DriverId { get; set; }

        [Range(1, 6)]
        public int MaxSeats { get; set; }

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;
    }

    public class VehicleUpdateDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string VehicleName { get; set; } = string.Empty;

        [Range(1, 6)]
        public int MaxSeats { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;
    }
}