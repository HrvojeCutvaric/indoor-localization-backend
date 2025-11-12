using IndoorLocalization.Models.Entities;
using System.Threading.Tasks;

namespace IndoorLocalization.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<User> GetByIdAsync(long id);
        Task UpdateTokenAsync(User user);

    }
}
