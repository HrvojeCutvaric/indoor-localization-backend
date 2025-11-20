using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Services.Interfaces
{
    public interface IAuthManager
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
        Task<User> RegisterAsync(RegisterRequestDto dto);
    }
}
