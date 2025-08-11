using Xunit;
using Moq;
using WebSIMS.Controllers;
using WebSIMS.Interfaces;
using WebSIMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSIMS.BDContext.Entities;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WebSIMS.Tests
{
    public class StudentControllerTests
    {
        private readonly Mock<IStudentRepository> _mockRepo;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _mockRepo = new Mock<IStudentRepository>();
            _controller = new StudentController(_mockRepo.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "testuser"),
                        new Claim(ClaimTypes.Role, "Admin")
                    }, "mock"))
                }
            };
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfStudents()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllStudentsAsync())
                .ReturnsAsync(GetTestStudents());

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<StudentListViewModel>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_WhenStudentNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetStudentByIdAsync(1))
                .ReturnsAsync((Students)null);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithStudent()
        {
            // Arrange
            var student = new Students { StudentID = 1, User = new Users { Username = "test" } };
            _mockRepo.Setup(repo => repo.GetStudentByIdAsync(1))
                .ReturnsAsync(student);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<StudentListViewModel>(viewResult.ViewData.Model);
            Assert.Equal(1, model.StudentID);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_POST_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            var studentModel = new CreateStudentViewModel();
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var result = await _controller.Create(studentModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(studentModel, viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_GET_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetStudentByIdAsync(1)).ReturnsAsync((Students)null);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_GET_ReturnsViewResult_WithStudent()
        {
            // Arrange
            var student = new Students { StudentID = 1, User = new Users { Username = "test" } };
            _mockRepo.Setup(repo => repo.GetStudentByIdAsync(1)).ReturnsAsync(student);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EditStudentViewModel>(viewResult.Model);
            Assert.Equal(1, model.StudentID);
        }

        [Fact]
        public async Task Edit_POST_ReturnsNotFound_WhenIdDoesNotMatchModel()
        {
            // Arrange
            var model = new EditStudentViewModel { StudentID = 2 };

            // Act
            var result = await _controller.Edit(1, model);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_POST_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new EditStudentViewModel { StudentID = 1 };
            _controller.ModelState.AddModelError("Error", "Some error");

            // Act
            var result = await _controller.Edit(1, model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]

        private List<StudentListViewModel> GetTestStudents()
        {
            return new List<StudentListViewModel>
            {
                new StudentListViewModel { StudentID = 1, Username = "test1", FirstName = "Test", LastName = "One" },
                new StudentListViewModel { StudentID = 2, Username = "test2", FirstName = "Test", LastName = "Two" }
            };
        }
    }
}
