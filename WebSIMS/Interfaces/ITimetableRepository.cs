using WebSIMS.BDContext.Entities;
using WebSIMS.Models;

namespace WebSIMS.Interfaces
{
    public interface ITimetableRepository
    {
        Task<IEnumerable<Schedules>> GetAllSchedulesAsync();
        Task<Schedules> GetScheduleByIdAsync(int id);
        Task<IEnumerable<Schedules>> GetSchedulesByClassIdAsync(int classId);
        Task<IEnumerable<Schedules>> GetSchedulesByDayOfWeekAsync(string dayOfWeek);
        Task<bool> CreateScheduleAsync(Schedules schedule);
        Task<bool> UpdateScheduleAsync(Schedules schedule);
        Task<bool> DeleteScheduleAsync(int id);
        Task<bool> ScheduleExistsAsync(int id);
        Task<IEnumerable<TimetableViewModel>> GetTimetableDataAsync();
        Task<IEnumerable<TimetableViewModel>> GetTimetableByWeekAsync(DateTime weekStart);
    }
} 