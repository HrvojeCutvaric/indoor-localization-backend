using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Security.Interfaces;
using IndoorLocalization.Services.Interfaces;
using System.Security.Cryptography;

namespace IndoorLocalization.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IPasswordHasher _passwordHasher;

        public OtpService(IOtpRepository otpRepository, IPasswordHasher passwordHasher)
        {
            _otpRepository = otpRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> GenerateAndStoreOtpAsync(long userId)
        {
            var random = RandomNumberGenerator.GetInt32(100000, 1000000);
            string otp = random.ToString("D6");

            var salt = _passwordHasher.GenerateSalt();
            var hash = _passwordHasher.HashPassword(otp, salt);

            var otpEntity = new OtpCode
            {
                UserId = userId,
                CodeHash = $"{salt}:{hash}",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                Attempts = 0,
                IsUsed = false
            };

            await _otpRepository.AddAsync(otpEntity);

            return otp;
        }

        public async Task<long?> ValidateOtpAsync(long userId, string otp)
        {
            var activeOtp = await _otpRepository.GetLatestByUserIdAsync(userId);

            if (activeOtp == null)
            {
                return -1;
            }

            if (activeOtp.IsUsed)
            {
                return -2;
            }

            if (activeOtp.ExpiresAt < DateTime.UtcNow)
            {
                return -3;
            }

            activeOtp.Attempts++;

            if (activeOtp.Attempts > 3)
            {
                activeOtp.IsUsed = true;
                await _otpRepository.UpdateAsync(activeOtp);
                return -4;
            }
            await _otpRepository.UpdateAsync(activeOtp);

            var parts = activeOtp.CodeHash.Split(':');
            if (parts.Length != 2) return -1;

            var salt = parts[0];
            var storedHash = parts[1];

            var computedHash = _passwordHasher.HashPassword(otp, salt);

            var isValid = CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(storedHash),
                Convert.FromBase64String(computedHash)
            );

            if (isValid)
            {
                activeOtp.IsUsed = true;
                await _otpRepository.UpdateAsync(activeOtp);
                return userId;
            }

            return -1;
        }
    }
}
