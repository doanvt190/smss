using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using BCrypt.Net;

namespace WebSIMS.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository repository)
        {
            _userRepository = repository;
        }
        public async Task<Users> LoginUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null) return null;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
        }

        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsername(username);
        }

        public async Task CreateUserAsync(Users user)
        {
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangeAsync();
        }
    }
}
