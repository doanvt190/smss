using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using WebSIMS.Models;

namespace WebSIMS.Repositories
{
    public class TimetableRepository : ITimetableRepository
    {
        private readonly SIMSDBContext _context;

        public TimetableRepository(SIMSDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedules>> GetAllSchedulesAsync()
        {
            return await _context.SchedulesDb
                .Include(s => s.Class)
                .ThenInclude(c => c.Course)
                .Include(s => s.Class)
                .ThenInclude(c => c.Faculty)
                .ToListAsync();
        }

        public async Task<Schedules> GetScheduleByIdAsync(int id)
        {
            return await _context.SchedulesDb
                .Include(s => s.Class)
                .ThenInclude(c => c.Course)
                .Include(s => s.Class)
                .ThenInclude(c => c.Faculty)
                .FirstOrDefaultAsync(s => s.ScheduleID == id);
        }

        public async Task<IEnumerable<Schedules>> GetSchedulesByClassIdAsync(int classId)
        {
            return await _context.SchedulesDb
                .Include(s => s.Class)
                .ThenInclude(c => c.Course)
                .Include(s => s.Class)
                .ThenInclude(c => c.Faculty)
                .Where(s => s.ClassID == classId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedules>> GetSchedulesByDayOfWeekAsync(string dayOfWeek)
        {
            return await _context.SchedulesDb
                .Include(s => s.Class)
                .ThenInclude(c => c.Course)
                .Include(s => s.Class)
                .ThenInclude(c => c.Faculty)
                .Where(s => s.DayOfWeek == dayOfWeek)
                .ToListAsync();
        }

        public async Task<bool> CreateScheduleAsync(Schedules schedule)
        {
            try
            {
                _context.SchedulesDb.Add(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateScheduleAsync(Schedules schedule)
        {
            try
            {
                _context.SchedulesDb.Update(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteScheduleAsync(int id)
        {
            try
            {
                var schedule = await _context.SchedulesDb.FindAsync(id);
                if (schedule != null)
                {
                    _context.SchedulesDb.Remove(schedule);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ScheduleExistsAsync(int id)
        {
            return await _context.SchedulesDb.AnyAsync(s => s.ScheduleID == id);
        }

        public async Task<IEnumerable<TimetableViewModel>> GetTimetableDataAsync()
        {
            var schedules = await _context.SchedulesDb
                .Include(s => s.Class)
                .ThenInclude(c => c.Course)
                .Include(s => s.Class)
                .ThenInclude(c => c.Faculty)
                .ToListAsync();

            var timetableData = schedules.Select(s => new TimetableViewModel
            {
                ScheduleID = s.ScheduleID,
                ClassID = s.ClassID,
                ClassName = s.Class.ClassName,
                CourseCode = s.Class.Course.CourseCode,
                CourseName = s.Class.Course.CourseName,
                FacultyName = $"{s.Class.Faculty.FirstName} {s.Class.Faculty.LastName}",
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Room = s.Room,
                Semester = s.Class.Semester,
                Year = s.Class.Year
            });

            return timetableData;
        }

        public async Task<IEnumerable<TimetableViewModel>> GetTimetableByWeekAsync(DateTime weekStart)
        {
            var schedules = await _context.SchedulesDb
                .Include(s => s.Class)
                .ThenInclude(c => c.Course)
                .Include(s => s.Class)
                .ThenInclude(c => c.Faculty)
                .ToListAsync();

            var timetableData = schedules.Select(s => new TimetableViewModel
            {
                ScheduleID = s.ScheduleID,
                ClassID = s.ClassID,
                ClassName = s.Class.ClassName,
                CourseCode = s.Class.Course.CourseCode,
                CourseName = s.Class.Course.CourseName,
                FacultyName = $"{s.Class.Faculty.FirstName} {s.Class.Faculty.LastName}",
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Room = s.Room,
                Semester = s.Class.Semester,
                Year = s.Class.Year
            });

            return timetableData;
        }
    }
} 