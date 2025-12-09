namespace IndoorLocalization.Services.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateAndStoreOtpAsync(long userId);
        Task<long?> ValidateOtpAsync(long userId, string otp);
    }
}
