using IndoorLocalization.Models.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace IndoorLocalization.Repositories
{
    public class MockUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public MockUserRepository()
        {
            var username = "test";
            var email = "test@test.com";
            var plainPassword = "Test@1234"; 
            var salt = GenerateSalt();
            var hashed = HashPassword(plainPassword, salt);

            _users.Add(new User
            {
                Id = 1,
                Username = username,
                PasswordHash = $"{salt}:{hashed}",
                Email = email,
                FirstName = "test",
                LastName = "test",
                RefreshToken = "testRefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(10)
            });
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<User?> GetByIdAsync(long id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task UpdateTokenAsync(User user)
        {
            var existing_user = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing_user != null)
            {
                existing_user.RefreshToken = user.RefreshToken;
                existing_user.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
            }
            return Task.CompletedTask;
        }

        public Task AddAsync(User user)
        {
            user.Id = (_users.Any() ? _users.Max(u => u.Id) : 0) + 1;
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            return Task.FromResult(_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<bool> ExistsByUsernameAsync(string username)
        {
            return Task.FromResult(_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));
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
