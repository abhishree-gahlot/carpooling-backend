using System;
using System.Threading.Tasks;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using CarpoolingSystem.Domain.Repositories;

namespace CarpoolingSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IVehicleRepository _vehicleRepository;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IVehicleRepository vehicleRepository)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _vehicleRepository = vehicleRepository;
        }

        public async Task RegisterAsync(RegisterRequestDto registerDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                throw new Exception("User already exists with this email.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                EmailId = registerDto.Email,
                PasswordHash = passwordHash,
                UserRole = registerDto.Role,
                UserName = registerDto.Username,
                DriverLicenseFile = registerDto.DriverLicenseFile,
                DriverLicenseFileName = registerDto.DriverLicenseFileName
            };

            if (registerDto.Role == UserRole.Passenger)
            {
                user.Pin = await GenerateUniquePinAsync();
            }

            await _userRepository.AddAsync(user);

            if (registerDto.Role == UserRole.Driver)
            {
                var vehicle = new Vehicle
                {
                    VehicleId = Guid.NewGuid(),
                    DriverId = user.UserId,
                    VehicleName = registerDto.VehicleName ?? "Unknown Vehicle",
                    MaxSeats = registerDto.MaxSeats ?? 4,
                    LicensePlate = registerDto.VehicleLicense ?? "UNKNOWN",
                    IsActive = true
                };

                await _vehicleRepository.AddAsync(vehicle);
                await _vehicleRepository.SaveChangesAsync();
            }
        }

        public async Task<string> LoginAsync(LoginRequestDto loginDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (existingUser == null)
            {
                throw new Exception("Invalid login credentials");
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.PasswordHash);

            if (!validPassword)
            {
                throw new Exception("Invalid login credentials");
            }

            return _tokenService.GenerateJwtToken(existingUser);
        }

        private async Task<string> GenerateUniquePinAsync()
        {
            var random = new Random();
            string pin;

            do
            {
                pin = random.Next(100000, 999999).ToString();
            }
            while (await _userRepository.PinExistsAsync(pin));

            return pin;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }
    }
}