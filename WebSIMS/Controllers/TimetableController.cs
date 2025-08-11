using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebSIMS.BDContext.Entities;
using WebSIMS.Interfaces;
using WebSIMS.Models;
using System.Linq;
using System.Security.Claims;

namespace WebSIMS.Controllers
{
    [Authorize]
    public class TimetableController : Controller
    {
        private readonly ITimetableRepository _timetableRepository;
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;

        public TimetableController(ITimetableRepository timetableRepository, IClassRepository classRepository, IUserRepository userRepository)
        {
            _timetableRepository = timetableRepository;
            _classRepository = classRepository;
            _userRepository = userRepository;
        }

        // GET: Timetable
        [Authorize(Roles = "Admin,Faculty,Student")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var username = User.Identity.Name;
                var user = await _userRepository.GetUserByUsername(username);
                if (user == null)
                {
                    return Challenge();
                }

                var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                
                IEnumerable<TimetableViewModel> timetableData;

                if (User.IsInRole("Student"))
                {
                    timetableData = await _timetableRepository.GetTimetableDataByStudentUserIdAsync(user.UserID);
                }
                else if (User.IsInRole("Faculty"))
                {
                    timetableData = await _timetableRepository.GetTimetableDataByFacultyUserIdAsync(user.UserID);
                }
                else // Admin
                {
                    timetableData = await _timetableRepository.GetTimetableDataAsync();
                }


                Console.WriteLine($"Loaded {timetableData.Count()} timetable entries");
                
                var viewModel = new TimetableDisplayViewModel
                {
                    WeekStart = weekStart,
                    WeekEnd = weekStart.AddDays(6),
                    TimetableData = [.. timetableData]
                };

                // Build timetable grid
                BuildTimetableGrid(viewModel);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading timetable: {ex.Message}");
                ModelState.AddModelError("", "Error loading timetable: " + ex.Message);
                return View(new TimetableDisplayViewModel());
            }
        }

        // GET: Timetable/Create
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var classes = await _classRepository.GetAllClassesAsync();
                var viewModel = new CreateTimetableViewModel
                {
                    AvailableClasses = [.. classes]
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading classes: " + ex.Message);
                return View(new CreateTimetableViewModel());
            }
        }

        // POST: Timetable/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create(CreateTimetableViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Log the model data for debugging
                    Console.WriteLine($"Creating schedule: ClassID={model.ClassID}, Day={model.DayOfWeek}, Start={model.StartTime}, End={model.EndTime}, Room={model.Room}");
                    
                    var schedule = new Schedules
                    {
                        ClassID = model.ClassID,
                        DayOfWeek = model.DayOfWeek,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        Room = model.Room
                    };

                    var success = await _timetableRepository.CreateScheduleAsync(schedule);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Timetable entry created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create timetable entry. Please check your database connection.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating timetable entry: {ex.Message}");
                    ModelState.AddModelError("", "Error creating timetable entry: " + ex.Message);
                }
            }
            else
            {
                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
            }

            // Reload classes for dropdown if validation fails
            try
            {
                var classes = await _classRepository.GetAllClassesAsync();
                model.AvailableClasses = [.. classes];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reloading classes: {ex.Message}");
                // Ignore error when reloading classes
            }

            return View(model);
        }

        // GET: Timetable/Edit/5
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var schedule = await _timetableRepository.GetScheduleByIdAsync(id);
                if (schedule == null)
                {
                    return NotFound();
                }

                var classes = await _classRepository.GetAllClassesAsync();
                var viewModel = new EditTimetableViewModel
                {
                    ScheduleID = schedule.ScheduleID,
                    ClassID = schedule.ClassID,
                    DayOfWeek = schedule.DayOfWeek,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                    Room = schedule.Room,
                    AvailableClasses = [.. classes]
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading timetable entry: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Timetable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Edit(int id, EditTimetableViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var schedule = await _timetableRepository.GetScheduleByIdAsync(id);
                    if (schedule == null)
                    {
                        return NotFound();
                    }

                    schedule.ClassID = model.ClassID;
                    schedule.DayOfWeek = model.DayOfWeek;
                    schedule.StartTime = model.StartTime;
                    schedule.EndTime = model.EndTime;
                    schedule.Room = model.Room;

                    var success = await _timetableRepository.UpdateScheduleAsync(schedule);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Timetable entry updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update timetable entry.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating timetable entry: " + ex.Message);
                }
            }

            // Reload classes for dropdown if validation fails
            try
            {
                var classes = await _classRepository.GetAllClassesAsync();
                model.AvailableClasses = [.. classes];
            }
            catch
            {
                // Ignore error when reloading classes
            }

            return View(model);
        }

        // POST: Timetable/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _timetableRepository.DeleteScheduleAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Timetable entry deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete timetable entry.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting timetable entry: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private void BuildTimetableGrid(TimetableDisplayViewModel viewModel)
        {
            var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            
            // Define the time slot mappings
            var timeSlotMappings = new Dictionary<string, (TimeSpan start, TimeSpan end)>
            {
                { "Time slot 1 (7:15-9:15)", (new TimeSpan(7, 15, 0), new TimeSpan(9, 15, 0)) },
                { "Time slot 2 (9:25-11:25)", (new TimeSpan(9, 25, 0), new TimeSpan(11, 25, 0)) },
                { "Time slot 3 (12:00-14:00)", (new TimeSpan(12, 0, 0), new TimeSpan(14, 0, 0)) },
                { "Time slot 4 (14:10-16:10)", (new TimeSpan(14, 10, 0), new TimeSpan(16, 10, 0)) },
                { "Time slot 5 (16:20-18:20)", (new TimeSpan(16, 20, 0), new TimeSpan(18, 20, 0)) }
            };
            
            foreach (var day in daysOfWeek)
            {
                viewModel.TimetableGrid[day] = new Dictionary<string, TimetableViewModel>();
                
                foreach (var timeSlot in viewModel.TimeSlots)
                {
                    viewModel.TimetableGrid[day][timeSlot] = null;
                }
            }

            // Populate the grid with actual timetable data
            foreach (var entry in viewModel.TimetableData)
            {
                // Find the matching time slot for this entry
                string matchingTimeSlot = null;
                foreach (var slotMapping in timeSlotMappings)
                {
                    if (entry.StartTime >= slotMapping.Value.start && entry.EndTime <= slotMapping.Value.end)
                    {
                        matchingTimeSlot = slotMapping.Key;
                        break;
                    }
                }

                if (matchingTimeSlot != null && viewModel.TimetableGrid.ContainsKey(entry.DayOfWeek) && 
                    viewModel.TimetableGrid[entry.DayOfWeek].ContainsKey(matchingTimeSlot))
                {
                    viewModel.TimetableGrid[entry.DayOfWeek][matchingTimeSlot] = entry;
                }
            }
        }
    }
}