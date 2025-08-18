using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;

namespace WebSIMS.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly SIMSDBContext _context;

        public FacultyRepository(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Faculties>> GetAllFacultiesAsync()
        {
            return await _context.FacultiesDb
                .Include(f => f.User)
                .ToListAsync();
        }

        public async Task<Faculties?> GetFacultyByIdAsync(int id)
        {
            return await _context.FacultiesDb
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.FacultyID == id);
        }

        public async Task<Faculties?> GetFacultyByUserIdAsync(int userId)
        {
            return await _context.FacultiesDb
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.UserID == userId);
        }

        public async Task AddFacultyAsync(Faculties faculty)
        {
            await _context.FacultiesDb.AddAsync(faculty);
        }

        public async Task UpdateFacultyAsync(Faculties faculty)
        {
            _context.FacultiesDb.Update(faculty);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFacultyAsync(int id)
        {
            var faculty = await _context.FacultiesDb.FindAsync(id);
            if (faculty != null)
            {
                _context.FacultiesDb.Remove(faculty);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}