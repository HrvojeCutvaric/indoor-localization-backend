using IndoorLocalization.Models.Entities;
using System.Security.Claims;

namespace IndoorLocalization.Security.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
