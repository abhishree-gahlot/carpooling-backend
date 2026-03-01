using CarpoolingSystem.Domain.Entities;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}