using System;
using System.Text;
using CarpoolingSystem.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using CarpoolingSystem.Domain.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace CarpoolingSystem.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtLifespanMinutes;

        public JwtTokenService(IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");

            _jwtSecret = jwtSection["Key"] ?? throw new ArgumentNullException("Jwt: Key is missing.");
            _jwtIssuer = jwtSection["Issuer"] ?? throw new ArgumentNullException("Jwt: Issuer is missing.");
            _jwtAudience = jwtSection["Audience"] ?? throw new ArgumentNullException("Jwt: Audience is missing.");
            _jwtLifespanMinutes = int.Parse(jwtSection["ExpiryMinutes"] ?? "60");
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailId),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var TokenDescriptor = new SecurityTokenDescriptor
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

            var token = tokenHandler.CreateToken(TokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}