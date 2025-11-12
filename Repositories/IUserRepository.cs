using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<User> GetByIdAsync(long id);
        Task UpdateTokenAsync(User user);

    }
}
