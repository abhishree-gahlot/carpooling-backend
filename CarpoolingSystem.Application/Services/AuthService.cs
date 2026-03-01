using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CarpoolingSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
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
            };

            if (registerDto.Role == UserRole.Passenger)
            {
                user.Pin = await GenerateUniquePinAsync();
            }

            await _userRepository.AddAsync(user);
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
    }
}

