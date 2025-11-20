using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Security;
using IndoorLocalization.Security.Interfaces;
using IndoorLocalization.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace IndoorLocalization.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthManager(IUserRepository userRepository, IPasswordHasher passwordHasher, IUserService userService, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userService = userService;
            _jwtService = jwtService;
        }

        public async Task<User> RegisterAsync(RegisterRequestDto dto)
        {
            ValidateRequiredFields(dto);
            ValidateEmailFormat(dto.Email);
            ValidatePasswordStrength(dto.Password);

            await EnsureEmailNotTaken(dto.Email);
            await EnsureUsernameNotTaken(dto.Username);

            var salt = _passwordHasher.GenerateSalt();
            var hashedPassword = _passwordHasher.HashPassword(dto.Password, salt);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = $"{salt}:{hashedPassword}"
            };

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            if (dto == null)
                throw new ArgumentException("Login data is required.");

            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Username and password are required.");

            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null) return null;


            if (!VerifyPassword(dto.Password, user.PasswordHash))
            {
                return null;
            }

            var accessToken = _jwtService.CreateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userService.UpdateTokenAsync(user);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task<RefreshTokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(dto.AccessToken);
            if (principal == null) return null;

            var userId = long.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)
                                    ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            if (user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            var newAccessToken = _jwtService.CreateToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userService.UpdateTokenAsync(user);

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        private bool VerifyPassword(string password, string storedPasswordHash)
        {
            var parts = storedPasswordHash.Split(':');
            var salt = parts[0];
            var storedHash = parts[1];

            var computedHash = _passwordHasher.HashPassword(password, salt);

            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(storedHash),
                Convert.FromBase64String(computedHash)
            );
        }

        private static void ValidateRequiredFields(RegisterRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Email and password are required.");
        }

        private static void ValidateEmailFormat(string email)
        {
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");
        }

        private static void ValidatePasswordStrength(string password)
        {
            if (!Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$"))
                throw new ArgumentException(
                    "Password must be at least 8 characters long and contain an uppercase letter, a number, and a special character."
                );
        }

        private async Task EnsureEmailNotTaken(string email)
        {
            if (await _userRepository.ExistsByEmailAsync(email))
                throw new InvalidOperationException("Email already registered.");
        }

        private async Task EnsureUsernameNotTaken(string username)
        {
            if (await _userRepository.ExistsByUsernameAsync(username))
                throw new InvalidOperationException("Username already taken.");
        }
    }
}
