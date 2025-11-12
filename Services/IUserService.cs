using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Services
{
    public interface IUserService
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(long id);
        Task UpdateTokenAsync(User user);
        Task<User> RegisterAsync(RegisterRequestDto dto);

    }
}
