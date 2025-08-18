using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSIMS.Models;
using WebSIMS.Services;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace WebSIMS.Controllers
{
    [Authorize]
    public class FacultyController : Controller
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly UserService _userService;

        public FacultyController(IFacultyRepository facultyRepository, UserService userService)
        {
            _facultyRepository = facultyRepository;
            _userService = userService;
        }

        // GET: Faculty
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var faculties = await _facultyRepository.GetAllFacultiesAsync();
                var facultyViewModels = faculties.Select(f => new FacultyListViewModel
                {
                    FacultyID = f.FacultyID,
                    Username = f.User?.Username ?? "N/A",
                    FirstName = f.FirstName,
                    LastName = f.LastName,
                    Email = f.Email,
                    Phone = f.Phone,
                    Department = f.Department,
                    HireDate = f.HireDate,
                    Role = f.User?.Role ?? "N/A"
                }).ToList();

                return View(facultyViewModels);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading faculty members.";
                return View(new List<FacultyListViewModel>());
            }
        }

        // GET: Faculty/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateFacultyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if username already exists
                    if (await _userService.GetUserByUsernameAsync(model.Username) != null)
                    {
                        ModelState.AddModelError("Username", "Username already exists.");
                        return View(model);
                    }

                    // Create user account
                    var user = new Users
                    {
                        Username = model.Username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        Role = "Faculty"
                    };

                    await _userService.CreateUserAsync(user);

                    // Create faculty member
                    var faculty = new Faculties
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Phone = model.Phone,
                        Department = model.Department,
                        HireDate = model.HireDate,
                        UserID = user.UserID
                    };

                    await _facultyRepository.AddFacultyAsync(faculty);
                    await _facultyRepository.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Faculty member created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the faculty member.");
                }
            }

            return View(model);
        }

        // GET: Faculty/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var faculty = await _facultyRepository.GetFacultyByIdAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }

            var viewModel = new EditFacultyViewModel
            {
                FacultyID = faculty.FacultyID,
                Username = faculty.User?.Username ?? string.Empty,
                FirstName = faculty.FirstName,
                LastName = faculty.LastName,
                Email = faculty.Email,
                Phone = faculty.Phone,
                Department = faculty.Department,
                HireDate = faculty.HireDate
            };

            return View(viewModel);
        }

        // POST: Faculty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, EditFacultyViewModel model)
        {
            if (id != model.FacultyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var faculty = await _facultyRepository.GetFacultyByIdAsync(id);
                    if (faculty == null)
                    {
                        return NotFound();
                    }

                    if (!string.Equals(faculty.Email, model.Email, StringComparison.OrdinalIgnoreCase) &&
                        await _facultyRepository.FacultyEmailExistsAsync(model.Email, faculty.FacultyID))
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View(model);
                    }

                    faculty.FirstName = model.FirstName;
                    faculty.LastName = model.LastName;
                    faculty.Email = model.Email;
                    faculty.Phone = model.Phone;
                    faculty.Department = model.Department;
                    faculty.HireDate = model.HireDate;

                    if (!string.IsNullOrWhiteSpace(model.NewPassword))
                    {
                        // Strong password is validated by model attribute
                        faculty.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                    }

                    await _facultyRepository.UpdateFacultyAsync(faculty);

                    TempData["SuccessMessage"] = "Faculty updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("Email", "Email already exists (constraint).");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the faculty member.");
                }
            }

            return View(model);
        }

        // GET: Faculty/Details/5
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Details(int id)
        {
            var faculty = await _facultyRepository.GetFacultyByIdAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }

            var viewModel = new FacultyListViewModel
            {
                FacultyID = faculty.FacultyID,
                Username = faculty.User?.Username ?? "N/A",
                FirstName = faculty.FirstName,
                LastName = faculty.LastName,
                Email = faculty.Email,
                Phone = faculty.Phone,
                Department = faculty.Department,
                HireDate = faculty.HireDate,
                Role = faculty.User?.Role ?? "N/A"
            };

            return View(viewModel);
        }

        // GET: Faculty/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var faculty = await _facultyRepository.GetFacultyByIdAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }

            var viewModel = new FacultyListViewModel
            {
                FacultyID = faculty.FacultyID,
                Username = faculty.User?.Username ?? "N/A",
                FirstName = faculty.FirstName,
                LastName = faculty.LastName,
                Email = faculty.Email,
                Phone = faculty.Phone,
                Department = faculty.Department,
                HireDate = faculty.HireDate,
                Role = faculty.User?.Role ?? "N/A"
            };

            return View(viewModel);
        }

        // POST: Faculty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _facultyRepository.DeleteFacultyAsync(id);
                await _facultyRepository.SaveChangesAsync();

                TempData["SuccessMessage"] = "Faculty member deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the faculty member.";
                return RedirectToAction(nameof(Index));
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}