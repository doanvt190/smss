using Xunit;
using Moq;
using WebSIMS.Repositories;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSIMS.Tests
{
    public class StudentRepositoryTests
    {
        private readonly SIMSDBContext _context;
        private readonly StudentRepository _repository;

        public StudentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<SIMSDBContext>()
                .UseInMemoryDatabase(databaseName: "TestSIMSDB")
                .Options;
            _context = new SIMSDBContext(options);
            _repository = new StudentRepository(_context);
        }

        private async Task SeedDatabase()
        {
            _context.Database.EnsureDeleted();
            var users = new List<Users>
            {
                new Users { UserID = 1, Username = "testuser1", PasswordHash = "hash", Role = "Student" },
                new Users { UserID = 2, Username = "testuser2", PasswordHash = "hash", Role = "Student" }
            };
            _context.UsersDb.AddRange(users);

            var students = new List<Students>
            {
                new Students { StudentID = 1, UserID = 1, FirstName = "Test", LastName = "One", Email = "test1@test.com" },
                new Students { StudentID = 2, UserID = 2, FirstName = "Test", LastName = "Two", Email = "test2@test.com" }
            };
            _context.StudentsDb.AddRange(students);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllStudentsAsync_ReturnsAllStudents()
        {
            // Arrange
            await SeedDatabase();

            // Act
            var result = await _repository.GetAllStudentsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetStudentByIdAsync_ReturnsStudent_WhenStudentExists()
        {
            // Arrange
            await SeedDatabase();

            // Act
            var result = await _repository.GetStudentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.StudentID);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ReturnsNull_WhenStudentDoesNotExist()
        {
            // Arrange
            await SeedDatabase();

            // Act
            var result = await _repository.GetStudentByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]

        public async Task UpdateStudentAsync_UpdatesStudent()
        {
            // Arrange
            await SeedDatabase();
            var student = await _repository.GetStudentByIdAsync(1);
            student.FirstName = "Updated";

            // Act
            var result = await _repository.UpdateStudentAsync(student);
            var updatedStudent = await _repository.GetStudentByIdAsync(1);

            // Assert
            Assert.True(result);
            Assert.Equal("Updated", updatedStudent.FirstName);
        }

        [Fact]
        public async Task DeleteStudentAsync_RemovesStudentAndUser()
        {
            // Arrange
            await SeedDatabase();

            // Act
            var result = await _repository.DeleteStudentAsync(1);
            var student = await _repository.GetStudentByIdAsync(1);
            var user = await _context.UsersDb.FindAsync(1);

            // Assert
            Assert.True(result);
            Assert.Null(student);
            Assert.Null(user);
        }

        [Fact]
        public async Task StudentExistsAsync_ReturnsTrue_WhenEmailExists()
        {
            // Arrange
            await SeedDatabase();

            // Act
            var result = await _repository.StudentExistsAsync("test1@test.com");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UsernameExistsAsync_ReturnsTrue_WhenUsernameExists()
        {
            // Arrange
            await SeedDatabase();

            // Act
            var result = await _repository.UsernameExistsAsync("testuser1");

            // Assert
            Assert.True(result);
        }
    }
}
