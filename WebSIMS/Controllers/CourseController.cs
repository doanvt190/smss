using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using WebSIMS.Models;

namespace WebSIMS.Controllers
{
    [Authorize] // Require authentication for all actions
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // GET: Course
        [Authorize(Roles = "Admin,Student,Faculty")] // All authenticated users can view the list
        public async Task<IActionResult> Index()
        {
            try
            {
                var courses = await _courseRepository.GetAllCoursesAsync();
                var viewModels = courses.Select(c => new CourseListViewModel
                {
                    CourseID = c.CourseID,
                    CourseCode = c.CourseCode,
                    CourseName = c.CourseName,
                    Description = c.Description,
                    Credits = c.Credits,
                    Department = c.Department,
                    CreatedAt = c.CreatedAt
                }).ToList();

                return View(viewModels);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading courses: " + ex.Message);
                return View(new List<CourseListViewModel>());
            }
        }

        // GET: Course/Create
        [Authorize(Roles = "Admin")] // Only Admin can access create form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can create courses
        public async Task<IActionResult> Create(CreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if course code already exists
                    if (await _courseRepository.CourseCodeExistsAsync(model.CourseCode))
                    {
                        ModelState.AddModelError("CourseCode", "Course Code already exists.");
                        return View(model);
                    }

                    // Create course
                    var course = new Courses
                    {
                        CourseCode = model.CourseCode,
                        CourseName = model.CourseName,
                        Description = model.Description,
                        Credits = model.Credits,
                        Department = model.Department
                    };

                    var success = await _courseRepository.CreateCourseAsync(course);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Course created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create course.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating course: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Course/Details/5
        [Authorize(Roles = "Admin,Student,Faculty")] // All authenticated users can view details
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new CourseListViewModel
            {
                CourseID = course.CourseID,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Description = course.Description,
                Credits = course.Credits,
                Department = course.Department,
                CreatedAt = course.CreatedAt
            };

            return View(viewModel);
        }

        // GET: Course/Edit/5
        [Authorize(Roles = "Admin")] // Only Admin can access edit form
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new CreateCourseViewModel
            {
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Description = course.Description,
                Credits = course.Credits,
                Department = course.Department
            };

            return View(viewModel);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can update courses
        public async Task<IActionResult> Edit(int id, CreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var course = await _courseRepository.GetCourseByIdAsync(id);
                    if (course == null)
                    {
                        return NotFound();
                    }

                    // Check if course code already exists for a different course
                    var existingCourse = await _courseRepository.GetCourseByCodeAsync(model.CourseCode);
                    if (existingCourse != null && existingCourse.CourseID != id)
                    {
                        ModelState.AddModelError("CourseCode", "Course Code already exists.");
                        return View(model);
                    }

                    // Update course properties
                    course.CourseCode = model.CourseCode;
                    course.CourseName = model.CourseName;
                    course.Description = model.Description;
                    course.Credits = model.Credits;
                    course.Department = model.Department;

                    var success = await _courseRepository.UpdateCourseAsync(course);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Course updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update course.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating course: " + ex.Message);
                }
            }

            return View(model);
        }

        // POST: Course/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can delete courses
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _courseRepository.DeleteCourseAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Course deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete course.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting course: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
