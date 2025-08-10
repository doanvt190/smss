using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SIMSDBContext _context;

        public CourseRepository(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Courses>> GetAllCoursesAsync()
        {
            return await _context.CoursesDb
                .OrderBy(c => c.CourseCode)
                .ToListAsync();
        }

        public async Task<Courses> GetCourseByIdAsync(int id)
        {
            return await _context.CoursesDb
                .FirstOrDefaultAsync(c => c.CourseID == id);
        }

        public async Task<Courses> GetCourseByCodeAsync(string courseCode)
        {
            return await _context.CoursesDb
                .FirstOrDefaultAsync(c => c.CourseCode == courseCode);
        }

        public async Task<bool> CourseCodeExistsAsync(string courseCode)
        {
            return await _context.CoursesDb
                .AnyAsync(c => c.CourseCode == courseCode);
        }

        public async Task<bool> CreateCourseAsync(Courses course)
        {
            try
            {
                course.CreatedAt = DateTime.Now;
                _context.CoursesDb.Add(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCourseAsync(Courses course)
        {
            try
            {
                _context.CoursesDb.Update(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            try
            {
                var course = await _context.CoursesDb.FindAsync(id);
                if (course != null)
                {
                    _context.CoursesDb.Remove(course);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
} 