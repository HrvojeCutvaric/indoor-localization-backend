using IndoorLocalization.Models.Entities;
using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Repositories;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace IndoorLocalization.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task UpdateTokenAsync(User user)
        {
            await _userRepository.UpdateTokenAsync(user);
        }

        public async Task<User> RegisterAsync(RegisterRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Email and password are required.");

            if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");

            if (!Regex.IsMatch(dto.Password, @"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$"))
                throw new ArgumentException("Password must be at least 8 characters long and contain an uppercase letter, a number, and a special character.");

            if (await _userRepository.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException("Email already registered.");

            if (await _userRepository.ExistsByUsernameAsync(dto.Username))
                throw new InvalidOperationException("Username already taken.");

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(dto.Password, salt);

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
            var refreshToken = _jwtService.RefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 
            await UpdateTokenAsync(user);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        private static bool VerifyPassword(string password, string storedPasswordHash)
        {
            if (string.IsNullOrEmpty(storedPasswordHash))
                return false;

            // expected format: "salt:hash"
            var parts = storedPasswordHash.Split(':');
            if (parts.Length != 2)
            {
                // support legacy plain text? return false (do not accept plain text)
                return false;
            }

            var salt = parts[0];
            var storedHash = parts[1];

            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
            var computedHash = Convert.ToBase64String(pbkdf2.GetBytes(32));

            // constant time comparison would be better; using simple compare here
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(computedHash),
                Convert.FromBase64String(storedHash)
            );
        }
        private static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private static string HashPassword(string password, string salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(pbkdf2.GetBytes(32));
        }
    }
}
