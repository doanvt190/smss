using WebSIMS.BDContext.Entities;

namespace WebSIMS.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Courses>> GetAllCoursesAsync();
        Task<Courses> GetCourseByIdAsync(int id);
        Task<Courses> GetCourseByCodeAsync(string courseCode);
        Task<bool> CourseCodeExistsAsync(string courseCode);
        Task<bool> CreateCourseAsync(Courses course);
        Task<bool> UpdateCourseAsync(Courses course);
        Task<bool> DeleteCourseAsync(int id);
    }
} 