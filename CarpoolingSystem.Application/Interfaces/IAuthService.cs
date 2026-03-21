using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequestDto loginDto);
        Task RegisterAsync(RegisterRequestDto registerDto);
    }
}