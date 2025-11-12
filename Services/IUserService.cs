using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Services
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> RegisterAsync(RegisterRequestDto dto);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
        Task UpdateTokenAsync(User user);

    }
}
