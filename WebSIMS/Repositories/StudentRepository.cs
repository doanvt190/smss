using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using WebSIMS.Models;

namespace WebSIMS.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SIMSDBContext _context;

        public StudentRepository(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentListViewModel>> GetAllStudentsAsync()
        {
            return await _context.StudentsDb
                .Include(s => s.User)
                .Select(s => new StudentListViewModel
                {
                    StudentID = s.StudentID,
                    Username = s.User.Username,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    DateOfBirth = s.DateOfBirth,
                    Gender = s.Gender,
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    EnrollmentDate = s.EnrollmentDate,
                    Program = s.Program,
                    CreatedAt = s.User.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<Students?> GetStudentByIdAsync(int studentId)
        {
            return await _context.StudentsDb
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentID == studentId);
        }

        public async Task<Students?> GetStudentByEmailAsync(string email)
        {
            return await _context.StudentsDb
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Students?> GetStudentByUsernameAsync(string username)
        {
            return await _context.StudentsDb
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.User.Username == username);
        }

        public async Task<Students> CreateStudentAsync(Students student, Users user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add user first
                _context.UsersDb.Add(user);
                await _context.SaveChangesAsync();

                // Set the UserID for the student
                student.UserID = user.UserID;
                student.EnrollmentDate = DateTime.Now;

                // Add student
                _context.StudentsDb.Add(student);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return student;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateStudentAsync(Students student)
        {
            try
            {
                _context.StudentsDb.Update(student);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            try
            {
                var student = await _context.StudentsDb
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.StudentID == studentId);

                if (student == null)
                    return false;

                _context.StudentsDb.Remove(student);
                _context.UsersDb.Remove(student.User);
                
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> StudentExistsAsync(string email)
        {
            return await _context.StudentsDb.AnyAsync(s => s.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.UsersDb.AnyAsync(u => u.Username == username);
        }

        public Task<Students?> GetStudentByUserIdAsync(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StudentListViewModel>> GetStudentsByFacultyUserIdAsync(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsStudentInFacultyClassesAsync(int id, int userID)
        {
            throw new NotImplementedException();
        }
    }
} 