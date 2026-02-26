using CarpoolingSystem.Application.DTOs;
using System.Threading.Tasks;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequestDto loginDto);
        Task RegisterAsync(RegisterRequestDto registerDto);
    }
}