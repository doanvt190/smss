using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using WebSIMS.Models;

namespace WebSIMS.Controllers
{
    [Authorize] // Require authentication for all actions
    public class ClassController : Controller
    {
        private readonly IClassRepository _classRepository;

        public ClassController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        // GET: Class
        [Authorize(Roles = "Admin,Student,Faculty")] // All authenticated users can view the list
        public async Task<IActionResult> Index()
        {
            try
            {
                var classes = await _classRepository.GetAllClassesAsync();
                return View(classes);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading classes: " + ex.Message);
                return View(new List<ClassListViewModel>());
            }
        }

        // GET: Class/Create
        [Authorize(Roles = "Admin,Faculty")] // Admin and Faculty can create classes
        public async Task<IActionResult> Create()
        {
            try
            {
                var courses = await _classRepository.GetAllCoursesAsync();
                var faculties = await _classRepository.GetAllFacultiesAsync();

                var viewModel = new CreateClassViewModel
                {
                    Courses = courses.Select(c => new SelectListItem
                    {
                        Value = c.CourseID.ToString(),
                        Text = $"{c.CourseCode} - {c.CourseName}"
                    }).ToList(),
                    Faculties = faculties.Select(f => new SelectListItem
                    {
                        Value = f.FacultyID.ToString(),
                        Text = $"{f.FirstName} {f.LastName}"
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading form data: " + ex.Message);
                return View(new CreateClassViewModel());
            }
        }

        // POST: Class/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")] // Admin and Faculty can create classes
        public async Task<IActionResult> Create(CreateClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var classEntity = new Classes
                    {
                        ClassName = model.ClassName,
                        CourseID = model.CourseID,
                        FacultyID = model.FacultyID,
                        Semester = model.Semester,
                        Year = model.Year
                    };

                    var success = await _classRepository.CreateClassAsync(classEntity);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Class created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create class.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating class: " + ex.Message);
                }
            }

            // Reload dropdown data if validation fails
            try
            {
                var courses = await _classRepository.GetAllCoursesAsync();
                var faculties = await _classRepository.GetAllFacultiesAsync();

                model.Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.CourseID.ToString(),
                    Text = $"{c.CourseCode} - {c.CourseName}"
                }).ToList();
                model.Faculties = faculties.Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = $"{f.FirstName} {f.LastName}"
                }).ToList();
            }
            catch
            {
                // Ignore errors when reloading dropdown data
            }

            return View(model);
        }

        // GET: Class/Details/5
        [Authorize(Roles = "Admin,Student,Faculty")] // All authenticated users can view details
        public async Task<IActionResult> Details(int id)
        {
            var classEntity = await _classRepository.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }

            var viewModel = new ClassListViewModel
            {
                ClassID = classEntity.ClassID,
                ClassName = classEntity.ClassName,
                CourseName = classEntity.Course?.CourseName ?? "N/A",
                FacultyName = $"{classEntity.Faculty?.FirstName} {classEntity.Faculty?.LastName}".Trim(),
                Semester = classEntity.Semester,
                Year = classEntity.Year
            };

            return View(viewModel);
        }

        // GET: Class/Edit/5
        [Authorize(Roles = "Admin,Faculty")] // Admin and Faculty can edit classes
        public async Task<IActionResult> Edit(int id)
        {
            var classEntity = await _classRepository.GetClassByIdAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }

            var courses = await _classRepository.GetAllCoursesAsync();
            var faculties = await _classRepository.GetAllFacultiesAsync();

            var viewModel = new CreateClassViewModel
            {
                ClassName = classEntity.ClassName,
                CourseID = classEntity.CourseID,
                FacultyID = classEntity.FacultyID,
                Semester = classEntity.Semester,
                Year = classEntity.Year,
                Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.CourseID.ToString(),
                    Text = $"{c.CourseCode} - {c.CourseName}"
                }).ToList(),
                Faculties = faculties.Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = $"{f.FirstName} {f.LastName}"
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Class/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")] // Admin and Faculty can edit classes
        public async Task<IActionResult> Edit(int id, CreateClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var classEntity = await _classRepository.GetClassByIdAsync(id);
                    if (classEntity == null)
                    {
                        return NotFound();
                    }

                    // Update class properties
                    classEntity.ClassName = model.ClassName;
                    classEntity.CourseID = model.CourseID;
                    classEntity.FacultyID = model.FacultyID;
                    classEntity.Semester = model.Semester;
                    classEntity.Year = model.Year;

                    var success = await _classRepository.UpdateClassAsync(classEntity);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Class updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update class.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating class: " + ex.Message);
                }
            }

            // Reload dropdown data if validation fails
            try
            {
                var courses = await _classRepository.GetAllCoursesAsync();
                var faculties = await _classRepository.GetAllFacultiesAsync();

                model.Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.CourseID.ToString(),
                    Text = $"{c.CourseCode} - {c.CourseName}"
                }).ToList();
                model.Faculties = faculties.Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = $"{f.FirstName} {f.LastName}"
                }).ToList();
            }
            catch
            {
                // Ignore errors when reloading dropdown data
            }

            return View(model);
        }

        // POST: Class/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can delete classes
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _classRepository.DeleteClassAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Class deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete class.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting class: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Class/Enroll/5
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Enroll(int id)
        {
            try
            {
                var classEntity = await _classRepository.GetClassByIdAsync(id);
                if (classEntity == null)
                {
                    return NotFound();
                }

                var students = await _classRepository.GetAllStudentsAsync();

                var viewModel = new EnrollStudentViewModel
                {
                    ClassID = id,
                    Students = students.Select(s => new SelectListItem
                    {
                        Value = s.StudentID.ToString(),
                        Text = $"{s.FirstName} {s.LastName} - {s.Email}"
                    }).ToList()
                };

                ViewBag.ClassName = classEntity.ClassName;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading enrollment form: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Class/Enroll/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Enroll(int id, EnrollStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if student is already enrolled
                    if (await _classRepository.IsStudentEnrolledAsync(model.StudentID, id))
                    {
                        ModelState.AddModelError("", "Student is already enrolled in this class.");
                    }
                    else
                    {
                        var enrollment = new StudentClassEnrollments
                        {
                            StudentID = model.StudentID,
                            ClassID = id,
                            Status = model.Status,
                            EnrollmentDate = DateTime.Now
                        };

                        var success = await _classRepository.EnrollStudentAsync(enrollment);
                        if (success)
                        {
                            TempData["SuccessMessage"] = "Student enrolled successfully!";
                            return RedirectToAction(nameof(Enrollments), new { id = id });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to enroll student.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error enrolling student: " + ex.Message);
                }
            }

            // Reload dropdown data if validation fails
            try
            {
                var students = await _classRepository.GetAllStudentsAsync();
                model.Students = students.Select(s => new SelectListItem
                {
                    Value = s.StudentID.ToString(),
                    Text = $"{s.FirstName} {s.LastName} - {s.Email}"
                }).ToList();
            }
            catch
            {
                // Ignore errors when reloading dropdown data
            }

            return View(model);
        }

        // GET: Class/Enrollments/5
        [Authorize(Roles = "Admin,Student,Faculty")]
        public async Task<IActionResult> Enrollments(int id)
        {
            try
            {
                var classEntity = await _classRepository.GetClassByIdAsync(id);
                if (classEntity == null)
                {
                    return NotFound();
                }

                var enrollments = await _classRepository.GetClassEnrollmentsAsync(id);
                ViewBag.ClassName = classEntity.ClassName;
                ViewBag.ClassID = id;

                return View(enrollments);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading enrollments: " + ex.Message);
                return View(new List<ClassEnrollmentViewModel>());
            }
        }

        // POST: Class/RemoveEnrollment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> RemoveEnrollment(int enrollmentId, int classId)
        {
            try
            {
                var success = await _classRepository.RemoveEnrollmentAsync(enrollmentId);
                if (success)
                {
                    TempData["SuccessMessage"] = "Student removed from class successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to remove student from class.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error removing student from class: " + ex.Message;
            }

            return RedirectToAction(nameof(Enrollments), new { id = classId });
        }
    }
} 