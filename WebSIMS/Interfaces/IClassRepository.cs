using WebSIMS.BDContext.Entities;
using WebSIMS.Models;

namespace WebSIMS.Interfaces
{
    public interface IClassRepository
    {
        Task<IEnumerable<ClassListViewModel>> GetAllClassesAsync();
        Task<Classes> GetClassByIdAsync(int id);
        Task<bool> CreateClassAsync(Classes classEntity);
        Task<bool> UpdateClassAsync(Classes classEntity);
        Task<bool> DeleteClassAsync(int id);
        Task<IEnumerable<Courses>> GetAllCoursesAsync();
        Task<IEnumerable<Faculties>> GetAllFacultiesAsync();
        Task<IEnumerable<Students>> GetAllStudentsAsync();
        Task<bool> EnrollStudentAsync(StudentClassEnrollments enrollment);
        Task<bool> RemoveEnrollmentAsync(int enrollmentId);
        Task<IEnumerable<ClassEnrollmentViewModel>> GetClassEnrollmentsAsync(int classId);
        Task<bool> IsStudentEnrolledAsync(int studentId, int classId);
    }
} 