using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using WebSIMS.Models;
using BCrypt.Net;
using System.Security.Claims;

namespace WebSIMS.Controllers
{
    [Authorize] // Require authentication for all actions
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;

        public StudentController(IStudentRepository studentRepository, IUserRepository userRepository)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
        }

        // GET: Student
        [Authorize(Roles = "Admin,Faculty")] // All authenticated users can view the list
        public async Task<IActionResult> Index()
        {
            try
            {
                var username = User.Identity.Name;
                var user = await _userRepository.GetUserByUsername(username);

                if (user == null)
                {
                    return Challenge(); // or handle as appropriate
                }

                if (User.IsInRole("Student"))
                {
                    var student = await _studentRepository.GetStudentByUserIdAsync(user.UserID);
                    if (student == null)
                    {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Details), new { id = student.StudentID });
                }

                IEnumerable<StudentListViewModel> students;
                if (User.IsInRole("Faculty"))
                {
                    students = await _studentRepository.GetStudentsByFacultyUserIdAsync(user.UserID);
                }
                else // Admin
                {
                    students = await _studentRepository.GetAllStudentsAsync();
                }

                return View(students);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading students: " + ex.Message);
                return View(new List<StudentListViewModel>());
            }
        }

        // GET: Student/Create
        [Authorize(Roles = "Admin")] // Only Admin can access create form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can create students
        public async Task<IActionResult> Create(CreateStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if username already exists
                    if (await _studentRepository.UsernameExistsAsync(model.Username))
                    {
                        ModelState.AddModelError("Username", "Username already exists.");
                        return View(model);
                    }

                    // Check if email already exists
                    if (await _studentRepository.StudentExistsAsync(model.Email))
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View(model);
                    }

                    // Create user
                    var user = new Users
                    {
                        Username = model.Username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        Role = "Student",
                        CreatedAt = DateTime.Now
                    };

                    // Create student
                    var student = new Students
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateOfBirth = model.DateOfBirth,
                        Gender = model.Gender,
                        Email = model.Email,
                        Phone = model.Phone,
                        Address = model.Address,
                        Program = model.Program
                    };

                    await _studentRepository.CreateStudentAsync(student, user);

                    TempData["SuccessMessage"] = "Student created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating student: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Student/Details/5
        [Authorize(Roles = "Admin,Student,Faculty")] // All authenticated users can view details
        public async Task<IActionResult> Details(int id)
        {
            var username = User.Identity.Name;
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return Challenge();
            }

            if (User.IsInRole("Student"))
            {
                var student = await _studentRepository.GetStudentByUserIdAsync(user.UserID);
                if (student == null || student.StudentID != id)
                {
                    return Forbid();
                }
            }
            else if (User.IsInRole("Faculty"))
            {
                var isAuthorized = await _studentRepository.IsStudentInFacultyClassesAsync(id, user.UserID);
                if (!isAuthorized)
                {
                    return Forbid();
                }
            }


            var studentDetails = await _studentRepository.GetStudentByIdAsync(id);
            if (studentDetails == null)
            {
                return NotFound();
            }

            var viewModel = new StudentListViewModel
            {
                StudentID = studentDetails.StudentID,
                Username = studentDetails.User.Username,
                FirstName = studentDetails.FirstName,
                LastName = studentDetails.LastName,
                DateOfBirth = studentDetails.DateOfBirth,
                Gender = studentDetails.Gender,
                Email = studentDetails.Email,
                Phone = studentDetails.Phone,
                Address = studentDetails.Address,
                EnrollmentDate = studentDetails.EnrollmentDate,
                Program = studentDetails.Program,
                CreatedAt = studentDetails.User.CreatedAt
            };

            return View(viewModel);
        }

        // GET: Student/Edit/5
        [Authorize(Roles = "Admin")] // Only Admin can access edit form
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new EditStudentViewModel
            {
                StudentID = student.StudentID,
                Username = student.User.Username,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                Email = student.Email,
                Phone = student.Phone,
                Address = student.Address,
                Program = student.Program
            };

            return View(viewModel);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can update students
        public async Task<IActionResult> Edit(int id, EditStudentViewModel model)
        {
            if (id != model.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var studentToUpdate = await _studentRepository.GetStudentByIdAsync(id);
                    if (studentToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Check if username is being changed and if the new one already exists
                    if (studentToUpdate.User.Username != model.Username && await _studentRepository.UsernameExistsAsync(model.Username))
                    {
                        ModelState.AddModelError("Username", "Username already exists.");
                        return View(model);
                    }

                    // Check if email is being changed and if the new one already exists
                    if (studentToUpdate.Email != model.Email && await _studentRepository.StudentExistsAsync(model.Email))
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View(model);
                    }

                    // Update user properties
                    studentToUpdate.User.Username = model.Username;
                    if (!string.IsNullOrWhiteSpace(model.NewPassword))
                    {
                        studentToUpdate.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                    }

                    // Update student properties
                    studentToUpdate.FirstName = model.FirstName;
                    studentToUpdate.LastName = model.LastName;
                    studentToUpdate.DateOfBirth = model.DateOfBirth;
                    studentToUpdate.Gender = model.Gender;
                    studentToUpdate.Email = model.Email;
                    studentToUpdate.Phone = model.Phone;
                    studentToUpdate.Address = model.Address;
                    studentToUpdate.Program = model.Program;

                    var success = await _studentRepository.UpdateStudentAsync(studentToUpdate);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Student updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update student.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating student: " + ex.Message);
                }
            }

            return View(model);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can delete students
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _studentRepository.DeleteStudentAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Student deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete student.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting student: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}