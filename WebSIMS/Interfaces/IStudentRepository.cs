using WebSIMS.BDContext.Entities;
using WebSIMS.Models;

namespace WebSIMS.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentListViewModel>> GetAllStudentsAsync();
        Task<Students?> GetStudentByIdAsync(int studentId);
        Task<Students?> GetStudentByEmailAsync(string email);
        Task<Students?> GetStudentByUsernameAsync(string username);
        Task<Students> CreateStudentAsync(Students student, Users user);
        Task<bool> UpdateStudentAsync(Students student);
        Task<bool> DeleteStudentAsync(int studentId);
        Task<bool> StudentExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<Students?> GetStudentByUserIdAsync(int userID);
        Task<IEnumerable<StudentListViewModel>> GetStudentsByFacultyUserIdAsync(int userID);
        Task<bool> IsStudentInFacultyClassesAsync(int id, int userID);
    }
}