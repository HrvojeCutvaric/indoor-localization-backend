using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        Task AddAsync(OtpCode otp);
        Task<OtpCode?> GetLatestByUserIdAsync(long userId);
        Task UpdateAsync(OtpCode otp);
    }
}
