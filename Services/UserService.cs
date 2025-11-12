using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using IndoorLocalization.Models.DTOs;

namespace IndoorLocalization.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
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
