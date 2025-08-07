using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;

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

            return user.PasswordHash.Equals(password) ? user : null;
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
