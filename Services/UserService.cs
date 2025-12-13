using IndoorLocalization.Models.Entities;
using IndoorLocalization.Models.DTOs;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using IndoorLocalization.Services.Interfaces;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Security;

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
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task UpdateTokenAsync(User user)
        {
            await _userRepository.UpdateTokenAsync(user);
        }
    }
}
