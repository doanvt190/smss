using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SIMSDBContext _dbContext;
        public UserRepository(SIMSDBContext context)
        {
            _dbContext = context;
        }
        public async Task AddAsync(Users user)
        {
            await _dbContext.UsersDb.AddAsync(user);
        }

        public async Task<Users?> GetUserById(int id)
        {
            return await _dbContext.UsersDb.FindAsync(id).AsTask();
        }

        public async Task<Users?> GetUserByUsername(string username)
        {
            return await _dbContext.UsersDb.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task DeleteAsync(Users user)
        {
            _dbContext.UsersDb.Remove(user);
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
