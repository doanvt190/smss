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
                TotalAdmins = await _context.UsersDb.CountAsync(u => u.Role == "Admin"),
                TotalStudents = await _context.StudentsDb.CountAsync(),
                TotalFaculty = await _context.FacultiesDb.CountAsync(),
                TotalCourses = await _context.CoursesDb.CountAsync(),
                TotalClasses = await _context.ClassesDb.CountAsync(),
                RecentStudents = await _context.StudentsDb
                    .OrderByDescending(s => s.EnrollmentDate)
                    .Take(5)
                    .Select(s => new StudentSummary
                    {
                        Name = $"{s.FirstName} {s.LastName}",
                        Email = s.Email,
                        Program = s.Program,
                        EnrollmentDate = s.EnrollmentDate
                    })
                    .ToListAsync(),
                RecentFaculty = await _context.FacultiesDb
                    .OrderByDescending(f => f.HireDate)
                    .Take(5)
                    .Select(f => new FacultySummary
                    {
                        Name = $"{f.FirstName} {f.LastName}",
                        Email = f.Email,
                        Department = f.Department,
                        HireDate = f.HireDate
                    })
                    .ToListAsync()
            };

            return View(dashboardViewModel);
        }
    }
}
