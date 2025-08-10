using WebSIMS.BDContext.Entities;

namespace WebSIMS.Interfaces
{
    public interface IUserRepository
    {
        Task<Users?> GetUserByUsername(string username);
        Task<Users?> GetUserById(int id);
        Task AddAsync(Users user);
        Task DeleteAsync(Users user);
        Task SaveChangeAsync();
    }
}
