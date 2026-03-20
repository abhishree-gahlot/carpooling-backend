using CarpoolingSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CarpoolingSystem.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<bool> PinExistsAsync(string pin);
        Task<User?> GetByIdAsync(Guid userId);
    }
}