using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using CarpoolingSystem.Domain.Enums;
using System.Threading.Tasks;

namespace CarpoolingSystem.Application.Services {
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtLifespanMinutes;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;

            var jwtSection = configuration.GetSection("Jwt"); _jwtSecret = jwtSection["Key"] ?? throw new ArgumentNullException("JWT Key missing in configuration");
            _jwtIssuer = jwtSection["Issuer"] ?? throw new ArgumentNullException("JWT Issuer missing in configuration");
            _jwtAudience = jwtSection["Audience"] ?? throw new ArgumentNullException("JWT Audience missing in configuration");
            _jwtLifespanMinutes = int.Parse(jwtSection["LifespanMinutes"] ?? "60"); 
        }

        public async Task RegisterAsync(RegisterRequestDto registerDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                throw new Exception("User already exists with this email.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            if (!Enum.TryParse<UserRole>(registerDto.Role, true, out var role))
            {
                role = UserRole.Passenger; 
            }
            var user = new User
            {
                EmailId = registerDto.Email,
                PasswordHash = passwordHash,
                UserRole = role,
                UserName = registerDto.Username,
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<string> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new Exception("Invalid login credentials");
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!validPassword)
            {
                throw new Exception("Invalid login credentials");
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailId),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtLifespanMinutes),
                Issuer = _jwtIssuer,       
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

