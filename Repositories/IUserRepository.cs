using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(long id);
        Task UpdateTokenAsync(User user);
    }
}
