using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories
{
    public class MockUserRepository : IUserRepository
    {

        private readonly List<User> _users = new List<User>();

        public MockUserRepository()
        {
            _users.Add(new User
            {
                Id = 1,
                Username = "test",
                PasswordHash = "testpassword",
                Email = "test@test.com",
                FirstName = "test",
                LastName = "test",
                RefreshToken = "testRefreshToken",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(10)
            });
        }

        public Task<User> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task<User> GetByIdAsync(long id)
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
    }
}
