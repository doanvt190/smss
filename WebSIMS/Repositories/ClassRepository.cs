using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using WebSIMS.Models;

namespace WebSIMS.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly SIMSDBContext _context;

        public ClassRepository(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassListViewModel>> GetAllClassesAsync()
        {
            var classes = await _context.ClassesDb
                .Include(c => c.Course)
                .Include(c => c.Faculty)
                .ToListAsync();

            return classes.Select(c => new ClassListViewModel
            {
                ClassID = c.ClassID,
                ClassName = c.ClassName,
                CourseCode = c.Course?.CourseCode ?? "N/A",
                CourseName = c.Course?.CourseName ?? "N/A",
                FacultyName = $"{c.Faculty?.FirstName} {c.Faculty?.LastName}".Trim(),
                Semester = c.Semester,
                Year = c.Year
            });
        }

        public async Task<Classes> GetClassByIdAsync(int id)
        {
            return await _context.ClassesDb
                .Include(c => c.Course)
                .Include(c => c.Faculty)
                .FirstOrDefaultAsync(c => c.ClassID == id);
        }

        public async Task<bool> CreateClassAsync(Classes classEntity)
        {
            try
            {
                _context.ClassesDb.Add(classEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateClassAsync(Classes classEntity)
        {
            try
            {
                _context.ClassesDb.Update(classEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteClassAsync(int id)
        {
            try
            {
                var classEntity = await _context.ClassesDb.FindAsync(id);
                if (classEntity != null)
                {
                    _context.ClassesDb.Remove(classEntity);
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

        public async Task<IEnumerable<Courses>> GetAllCoursesAsync()
        {
            return await _context.CoursesDb.ToListAsync();
        }

        public async Task<IEnumerable<Faculties>> GetAllFacultiesAsync()
        {
            return await _context.FacultiesDb.ToListAsync();
        }

        public async Task<IEnumerable<Students>> GetAllStudentsAsync()
        {
            return await _context.StudentsDb.ToListAsync();
        }

        public async Task<bool> EnrollStudentAsync(StudentClassEnrollments enrollment)
        {
            try
            {
                _context.StudentClassEnrollmentsDb.Add(enrollment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveEnrollmentAsync(int enrollmentId)
        {
            try
            {
                var enrollment = await _context.StudentClassEnrollmentsDb.FindAsync(enrollmentId);
                if (enrollment != null)
                {
                    _context.StudentClassEnrollmentsDb.Remove(enrollment);
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

        public async Task<IEnumerable<ClassEnrollmentViewModel>> GetClassEnrollmentsAsync(int classId)
        {
            var enrollments = await _context.StudentClassEnrollmentsDb
                .Include(e => e.Student)
                .Include(e => e.Class)
                .ThenInclude(c => c.Course)
                .Include(e => e.Class)
                .ThenInclude(c => c.Faculty)
                .Where(e => e.ClassID == classId)
                .ToListAsync();

            return enrollments.Select(e => new ClassEnrollmentViewModel
            {
                EnrollmentID = e.EnrollmentID,
                StudentName = $"{e.Student?.FirstName} {e.Student?.LastName}".Trim(),
                StudentEmail = e.Student?.Email ?? "N/A",
                ClassName = e.Class?.ClassName ?? "N/A",
                CourseName = e.Class?.Course?.CourseName ?? "N/A",
                FacultyName = $"{e.Class?.Faculty?.FirstName} {e.Class?.Faculty?.LastName}".Trim(),
                EnrollmentDate = e.EnrollmentDate,
                Status = e.Status
            });
        }

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int classId)
        {
            return await _context.StudentClassEnrollmentsDb
                .AnyAsync(e => e.StudentID == studentId && e.ClassID == classId && e.Status == "Active");
        }

        public Task<IEnumerable<ClassListViewModel>> GetClassesByStudentUserIdAsync(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClassListViewModel>> GetClassesByFacultyUserIdAsync(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsStudentEnrolledInClassByUserIdAsync(int userID, int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsFacultyAssignedToClassByUserIdAsync(int userID, int id)
        {
            throw new NotImplementedException();
        }
    }
} 