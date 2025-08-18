using WebSIMS.BDContext.Entities;

namespace WebSIMS.Interfaces
{
    public interface IFacultyRepository
    {
        Task<IEnumerable<Faculties>> GetAllFacultiesAsync();
        Task<Faculties?> GetFacultyByIdAsync(int id);
        Task<Faculties?> GetFacultyByUserIdAsync(int userId);
        Task AddFacultyAsync(Faculties faculty);
        Task UpdateFacultyAsync(Faculties faculty);
        Task DeleteFacultyAsync(int id);
        Task<bool> FacultyEmailExistsAsync(string email, int excludeFacultyId);
        Task SaveChangesAsync();
    }
}