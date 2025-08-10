using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;

namespace WebSIMS.Controllers
{
    public class TestController : Controller
    {
        private readonly SIMSDBContext _context;

        public TestController(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var result = new
                {
                    DatabaseConnection = "Connected",
                    FacultyTableExists = await CheckFacultyTableExists(),
                    FacultyCount = await GetFacultyCount(),
                    UsersWithFacultyRole = await GetUsersWithFacultyRole()
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        private async Task<bool> CheckFacultyTableExists()
        {
            try
            {
                var count = await _context.FacultiesDb.CountAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<int> GetFacultyCount()
        {
            try
            {
                return await _context.FacultiesDb.CountAsync();
            }
            catch
            {
                return -1;
            }
        }

        private async Task<List<object>> GetUsersWithFacultyRole()
        {
            try
            {
                var facultyUsers = await _context.UsersDb
                    .Where(u => u.Role == "Faculty")
                    .Select(u => new { u.UserID, u.Username, u.Role })
                    .ToListAsync();

                return facultyUsers.Cast<object>().ToList();
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}
