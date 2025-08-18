using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.Models;

namespace WebSIMS.Controllers
{
    [Authorize] // bat buoc phai dang nhap
    public class DashboardController : Controller
    {
        private readonly SIMSDBContext _context;

        public DashboardController(SIMSDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Student,Faculty")]
        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = new DashboardViewModel
            {
                SchoolName = "BTEC FPT School",
                SchoolDescription = "A leading institution providing high-quality education in technology and business",
            };

            if (User.IsInRole("Admin"))
            {
                dashboardViewModel.TotalAdmins = await _context.UsersDb.CountAsync(u => u.Role == "Admin");
                dashboardViewModel.TotalStudents = await _context.StudentsDb.CountAsync();
                dashboardViewModel.TotalFaculty = await _context.FacultiesDb.CountAsync();
                dashboardViewModel.TotalCourses = await _context.CoursesDb.CountAsync();
                dashboardViewModel.TotalClasses = await _context.ClassesDb.CountAsync();
                dashboardViewModel.RecentStudents = await _context.StudentsDb
                    .OrderByDescending(s => s.EnrollmentDate)
                    .Take(5)
                    .Select(s => new StudentSummary
                    {
                        Name = $"{s.FirstName} {s.LastName}",
                        Email = s.Email,
                        Program = s.Program,
                        EnrollmentDate = s.EnrollmentDate
                    })
                    .ToListAsync();

                dashboardViewModel.RecentFaculty = await _context.FacultiesDb
                    .OrderByDescending(f => f.HireDate)
                    .Take(5)
                    .Select(f => new FacultySummary
                    {
                        Name = $"{f.FirstName} {f.LastName}",
                        Email = f.Email,
                        Department = f.Department,
                        HireDate = f.HireDate
                    })
                    .ToListAsync();
            }
            else if (User.IsInRole("Faculty"))
            {
                var facultyUser = await _context.UsersDb.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
                if (facultyUser != null)
                {
                    var faculty = await _context.FacultiesDb.FirstOrDefaultAsync(f => f.UserID == facultyUser.UserID);
                    if (faculty != null)
                    {
                        var studentIdsInFacultyClasses = await _context.StudentClassEnrollmentsDb
                            .Where(e => e.Class.FacultyID == faculty.FacultyID)
                            .Select(e => e.StudentID)
                            .Distinct()
                            .ToListAsync();

                        dashboardViewModel.TotalStudents = studentIdsInFacultyClasses.Count;
                        dashboardViewModel.TotalClasses = await _context.ClassesDb.CountAsync(c => c.FacultyID == faculty.FacultyID);

                        dashboardViewModel.RecentStudents = await _context.StudentsDb
                            .Where(s => studentIdsInFacultyClasses.Contains(s.StudentID))
                            .OrderByDescending(s => s.EnrollmentDate)
                            .Take(5)
                            .Select(s => new StudentSummary
                            {
                                Name = $"{s.FirstName} {s.LastName}",
                                Email = s.Email,
                                Program = s.Program,
                                EnrollmentDate = s.EnrollmentDate
                            })
                            .ToListAsync();
                    }
                }
            }
            else if (User.IsInRole("Student"))
            {
                var studentUser = await _context.UsersDb.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
                if (studentUser != null)
                {
                    var student = await _context.StudentsDb.FirstOrDefaultAsync(s => s.UserID == studentUser.UserID);
                    if (student != null)
                    {
                        // Active classes for this student
                        dashboardViewModel.TotalClasses = await _context.StudentClassEnrollmentsDb
                            .CountAsync(e => e.StudentID == student.StudentID && e.Status == "Active");

                        // Courses for this student (distinct courses from active classes)
                        dashboardViewModel.TotalCourses = await _context.StudentClassEnrollmentsDb
                            .Where(e => e.StudentID == student.StudentID && e.Status == "Active")
                            .Select(e => e.Class.CourseID)
                            .Distinct()
                            .CountAsync();
                    }
                }
            }

            return View(dashboardViewModel);
        }
    }
}
